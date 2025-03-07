using System.Collections.Generic;
using UnityEngine;
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
        StartGame();
    }

    public void StartGame()
    {
        StartGameEvent?.Invoke();
        print("Starting game with mode: " + gameMode + " and map: " + map);
        if (gameMode == GameMode.freeForAll)
        {
            currentLives = maxLives;
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
            }
        }
        if (gameMode == GameMode.keepAway)
        {
            currentLives = 1;
            foreach (GameObject player in players)
            {
                player.transform.position = spawnPosition;
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

    public void PlayerDied(Damageable player)
    {
        if (gameMode == GameMode.freeForAll)
        {
            player.lives--;
            if (player.lives <= 0)
            {
                GameOver(player.gameObject);
                Destroy(player.gameObject);
            }
            else
            {
                RespawnPlayer(player.gameObject);
            }
        }
        if (gameMode == GameMode.keepAway)
        {

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

    private void GameOver(GameObject player)
    {
        Destroy(player);
        player.SetActive(false);
        if (AlivePlayers().Count <= 1)
        {
            EndGameEvent?.Invoke();
            print(AlivePlayers()[0].name + " is the winner");
            FindFirstObjectByType<PlayerCameraMovement>().WinScene(AlivePlayers()[0]);
        }
    }

    private List<GameObject> AlivePlayers()
    {
        List<GameObject> alivePlayers = new();

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy) alivePlayers.Add(player);
        }

        return alivePlayers;
    }
}
