using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int maxLives = 3;
    public int currentLives;
    public delegate void GameEvent();
    public event GameEvent StartGameEvent;
    public event GameEvent EndGameEvent;
    public static List<GameObject> players = new List<GameObject>();

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

    public void PlayerDied(GameObject player)
    {
        if (gameMode == GameMode.freeForAll)
        {
            currentLives--;
            if (currentLives <= 0)
            {
                GameOver(player);
            }
            else
            {
                RespawnPlayer(player);
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
        }
    }

    private void GameOver(GameObject player)
    {
        // Add game over screen
        player.SetActive(false);
        EndGameEvent?.Invoke();
    }
}
