using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int maxLives = 3;
    public int currentLives;
    public delegate void GameEvent();
    public event GameEvent StartGameEvent;
    public event GameEvent EndGameEvent;
    public static List<GameObject> players = new List<GameObject>();
    public static List<Color> playerColors = new List<Color>();
    public float offset = 1f;

    public static bool music = true;

    public bool gameOver = false;

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

    private void Start()
    {
        MusicManager.Instance.StartPlaylist();
        StartGame();
    }

    public void StartGame()
    {
        if (GameManager.players.Count == 0) return;

        StartGameEvent?.Invoke();
        print("Starting game with mode: " + gameMode + " and map: " + map);
        if (gameMode == GameMode.freeForAll)
        {
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition + (players.IndexOf(player) * Vector2.right * offset);
                player.GetComponent<Damageable>().lives = 5;
            }
        }
        if (gameMode == GameMode.keepAway)
        {
            currentLives = 1;
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
                player.GetComponent<Damageable>().lives = 0;
            }
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            currentLives = 1;
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
            }
        }
    }

    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    public static GameMode gameMode = GameMode.freeForAll; // loads a default gamemode as a safety net
    public static string map = "Platformer With Headroom"; // loads a default map as a safety net
    public Vector2 spawnPosition;
    public Vector2 hatSpawnPosition;

    public void PlayerDied(Damageable player)
    {
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
        if (gameMode == GameMode.keepAway)
        {
            RespawnPlayer(player.gameObject);
        }
        if (gameMode == GameMode.obstacleCourse)
        {

        }
    }

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

    private void GameOver()
    {
        gameOver = true;
        EndGameEvent?.Invoke();
        print(AlivePlayers()[0].name + " is the winner");
        FindFirstObjectByType<PlayerCameraMovement>().WinScene(AlivePlayers()[0]);
        WinScreen.Instance.ShowWinScreen(players.IndexOf(AlivePlayers()[0]) + 1);
        FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
    }

    public List<GameObject> AlivePlayers()
    {
        List<GameObject> alivePlayers = new();

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy) alivePlayers.Add(player);
        }

        return alivePlayers;
    }
}
