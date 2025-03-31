using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float time = 180f;
    public delegate void GameEvent();
    public event GameEvent StartGameEvent;
    public event GameEvent EndGameEvent;
    public static List<GameObject> players = new List<GameObject>();
    public static List<Color> playerColors = new List<Color>();
    public float offset = 1f;
    public static bool music = true;
    public bool gameOver = false;
    public GameTimer gameTimer;
    public static Dictionary<GameObject, float> playerHoldTimes = new Dictionary<GameObject, float>();
    public static GameMode gameMode = GameMode.freeForAll; // loads a default gamemode as a safety net
    public static string map = "Platformer With Headroom"; // loads a default map as a safety net
    public Vector2 spawnPosition;
    public List<Vector2> hatSpawnPositions = new List<Vector2>();
    public Canvas LeaderboardCanvas;
    public Canvas TimerCanvas;
    public GameObject hatObject;
    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    private void Awake() // Ensures only one instance of GameManager exists
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() // Starts the game and music
    {
        MusicManager.Instance.StartPlaylist();
        StartGame();
    }

    private void Update() // Continuously updates player hold times
    {
        if (gameOver) return;

        if (gameMode == GameMode.keepAway)
        {
            foreach (var player in players)
            {
                float holdTime = GetPlayerHoldTime(player);
                UpdatePlayerHoldTime(player, holdTime);
            }
        }
    }

    private float GetPlayerHoldTime(GameObject player)
    {
        UseItem useItem = player.GetComponent<UseItem>();
        if (useItem != null)
        {
            return useItem.holdTime;
        }
        return 0f;
    }

    public void StartGame() // Sets up the proper gamemode
    {
        GameManager.playerHoldTimes.Clear();
        if (GameManager.players.Count == 0) return;

        StartGameEvent?.Invoke();
        print("Starting game with mode: " + gameMode + " and map: " + map);
        if (gameMode == GameMode.freeForAll) // Sets up the game for free for all mode
        {
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                player.GetComponent<Damageable>().lives = 5;
            }
        }
        if (gameMode == GameMode.keepAway) // Sets up the game for keep away mode
        {
            gameTimer.startTime = time;
            gameTimer.StartTimer();
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                player.GetComponent<Damageable>().lives = 0;
            }
        }
        if (gameMode == GameMode.obstacleCourse) // Sets up the game for obstacle course mode
        {
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
                player.GetComponent<Damageable>().lives = 0;
            }
        }
    }

    public void PlayerDied(Damageable player) // Handles player deaths for the respective gamemode
    {
        UseItem useItem = player.GetComponent<UseItem>(); // Drop the item the player is holding
        if (useItem != null)
        {
            useItem.DropItem();
        }

        if (gameMode == GameMode.freeForAll) // Respawns player if they have lives left
        {
            player.lives--;
            if (player.lives <= 0 && !gameOver)
            {
                player.gameObject.SetActive(false);
                if (AlivePlayers().Count <= 1)
                {
                    GameOver(); // Winner is called when only one player is left
                }
            }
            else
            {
                RespawnPlayer(player.gameObject);
            }
        }
        if (gameMode == GameMode.keepAway) // Always respawns player regardless of lives
        {
            RespawnPlayer(player.gameObject);
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            RespawnPlayer(player.gameObject);
        }
    }

    private void RespawnPlayer(GameObject player) // Respawns player at the spawn point and resets health
    {
        RespawnOnTriggerEnter respawnScript = player.GetComponent<RespawnOnTriggerEnter>();
        if (respawnScript != null)
        {
            player.transform.position = respawnScript.spawnPoint;
            player.GetComponent<Damageable>().ResetDamage();
            player.GetComponent<Damageable>().Respawn();
        }
    }

    public void GameOver() // Ends game and displays winner
    {
        gameOver = true;
        EndGameEvent?.Invoke();
        LeaderboardCanvas.gameObject.SetActive(false);
        TimerCanvas.gameObject.SetActive(false);
        hatObject.SetActive(false);

        if (gameMode == GameMode.freeForAll) // Last player alive wins
        {
            GameObject winner = AlivePlayers()[0];
            print(winner.name + " is the winner");
            FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
            WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);
            FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
        }
        if (gameMode == GameMode.keepAway) // Player with the most time holding the hat wins
        {
            GameObject winner = null;
            float maxHoldTime = -1f;
            foreach (var player in GameManager.playerHoldTimes)
            {
                if (player.Value > maxHoldTime)
                {
                    maxHoldTime = player.Value;
                    winner = player.Key;
                }
            }
            if (winner != null)
            {
                print(winner.name + " is the winner with " + maxHoldTime + " seconds!");
                FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
                WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);
                FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
            }
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            GameObject winner = ObstacleCourse.playerWon;

            print(winner.name + " is the winner!");
            FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
            WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);
            FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
        }
    }

    public List<GameObject> AlivePlayers() // Returns a list of all players that are alive
    {
        List<GameObject> alivePlayers = new();

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy) alivePlayers.Add(player);
        }

        return alivePlayers;
    }

    public void UpdatePlayerHoldTime(GameObject player, float holdTime) // Updates the player's hold time and leaderboard
    {
        bool shouldSort = false;

        if (playerHoldTimes.ContainsKey(player))
        {
            if (holdTime > playerHoldTimes[player])
            {
                shouldSort = true;
            }
            playerHoldTimes[player] = holdTime;
        }
        else
        {
            playerHoldTimes.Add(player, holdTime);
            shouldSort = true;
        }
        LeaderboardManager.Instance.UpdatePlayerHoldTimeText(player, holdTime);
        if (shouldSort)
        {
            LeaderboardManager.Instance.UpdateLeaderboard();
        }
    }
}
