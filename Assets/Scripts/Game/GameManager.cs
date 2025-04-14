using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
/// <summary>
/// The GameManager class manages the overall game logic, including game modes, player states, 
/// game events, and game-over conditions. It ensures a single instance exists and provides 
/// functionality for starting, updating, and ending the game.
/// </summary>
/// <summary>
/// Manages the overall game logic, including game modes, player states, game events, and game-over conditions.
/// </summary>
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
    public Vector2 obstacleCourseSpawnPosition;
    public List<Vector2> hatSpawnPositions = new List<Vector2>();
    public Canvas LeaderboardCanvas;
    public Canvas TimerCanvas;
    public GameObject hatObject;

    /// <summary>
    /// Enum representing the different game modes.
    /// </summary>
    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    /// <summary>
    /// Ensures only one instance of GameManager exists.
    /// </summary>
    private void Awake()
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

    /// <summary>
    /// Starts the game and initializes music.
    /// </summary>
    private void Start()
    {
        MusicManager.Instance.StartPlaylist();
        StartGame();
    }

    /// <summary>
    /// Continuously updates player hold times during the game.
    /// </summary>
    private void Update()
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

    /// <summary>
    /// Retrieves the hold time of a player.
    /// </summary>
    /// <param name="player">The player GameObject.</param>
    /// <returns>The hold time of the player.</returns>
    private float GetPlayerHoldTime(GameObject player)
    {
        UseItem useItem = player.GetComponent<UseItem>();
        if (useItem != null)
        {
            return useItem.holdTime;
        }
        return 0f;
    }

    /// <summary>
    /// Sets up the game based on the selected game mode.
    /// </summary>
    public void StartGame()
    {
        GameManager.playerHoldTimes.Clear();
        if (GameManager.players.Count == 0) return;

        StartGameEvent?.Invoke();
        print("Starting game with mode: " + gameMode + " and map: " + map);
        if (gameMode == GameMode.freeForAll)
        {
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                player.GetComponent<Damageable>().lives = 5;
            }
        }
        if (gameMode == GameMode.keepAway)
        {
            gameTimer.startTime = time;
            gameTimer.StartTimer();
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                player.GetComponent<Damageable>().lives = 0;
            }
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
                player.GetComponent<Damageable>().lives = 0;
            }
        }
    }

    /// <summary>
    /// Handles player deaths based on the current game mode.
    /// </summary>
    /// <param name="player">The player that died.</param>
    public void PlayerDied(Damageable player)
    {
        UseItem useItem = player.GetComponent<UseItem>();
        if (useItem != null)
        {
            if (gameOver == false)
            {
                useItem.DropItem();
            }
            else
            {
                return;
            }
        }

        if (gameMode == GameMode.freeForAll)
        {
            player.lives--;
            if (player.lives <= 0 && !gameOver)
            {
                player.gameObject.SetActive(false);
                if (AlivePlayers().Count <= 1)
                {
                    GameOver();
                }
            }
            else
            {
                RespawnPlayer(player.gameObject);
            }
        }
        if (gameMode == GameMode.keepAway || gameMode == GameMode.obstacleCourse)
        {
            RespawnPlayer(player.gameObject);
        }
    }

    /// <summary>
    /// Respawns a player at their designated spawn point.
    /// </summary>
    /// <param name="player">The player GameObject to respawn.</param>
    private void RespawnPlayer(GameObject player)
    {
        RespawnOnTriggerEnter respawnScript = player.GetComponent<RespawnOnTriggerEnter>();
        if (respawnScript != null)
        {
            player.transform.position = respawnScript.spawnPoint;
            player.GetComponent<Damageable>().ResetDamage();
            player.GetComponent<Damageable>().Respawn();
        }
    }

    /// <summary>
    /// Ends the game and determines the winner based on the game mode.
    /// </summary>
    public void GameOver()
    {
        gameOver = true;
        EndGameEvent?.Invoke();
        LeaderboardCanvas.gameObject.SetActive(false);
        TimerCanvas.gameObject.SetActive(false);

        if (gameMode == GameMode.freeForAll)
        {
            GameObject winner = AlivePlayers()[0];
            print(winner.name + " is the winner");
            FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
            WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);
            FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
        }
        if (gameMode == GameMode.keepAway)
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
                StartCoroutine(MoveHatToWinner(winner));
                hatObject.SetActive(true);
                hatObject.GetComponent<Collider2D>().enabled = true;
                hatObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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

    /// <summary>
    /// Moves the hat to the winner in keep-away mode.
    /// </summary>
    /// <param name="winner">The winning player GameObject.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    private IEnumerator MoveHatToWinner(GameObject winner)
    {
        while (!winner.GetComponent<UseItem>().IsHoldingItem())
        {
            hatObject.transform.position = winner.transform.position + Vector3.up * 3 / 2;
            winner.GetComponent<UseItem>().PickUpItem(hatObject);
            yield return null;
        }
    }

    /// <summary>
    /// Returns a list of all players that are currently alive.
    /// </summary>
    /// <returns>A list of alive player GameObjects.</returns>
    public List<GameObject> AlivePlayers()
    {
        List<GameObject> alivePlayers = new();

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy) alivePlayers.Add(player);
        }

        return alivePlayers;
    }

    /// <summary>
    /// Updates the player's hold time and updates the leaderboard.
    /// </summary>
    /// <param name="player">The player GameObject.</param>
    /// <param name="holdTime">The hold time to update.</param>
    public void UpdatePlayerHoldTime(GameObject player, float holdTime)
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
