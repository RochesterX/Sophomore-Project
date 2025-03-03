using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int maxLives = 3;
    public int currentLives;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        print("Starting game with mode: " + gameMode + " and map: " + map);
        if (gameMode == GameMode.freeForAll)
        {
            currentLives = maxLives;
            StartFreeForAll();
        }
        if (gameMode == GameMode.keepAway)
        {
            currentLives = 1;
            StartKeepAway();
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            currentLives = 1;
            StartObstacleCourse();
        }
    }

    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    public static GameMode gameMode = GameMode.freeForAll;

    public static string map = "Platformer With Headroom"; //called for in PlayerManager and should be changed to load from here instead

    public static List<GameObject> players = new List<GameObject>();

    public Vector2 spawnPosition;

    private void StartFreeForAll()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
        }
    }

    private void StartKeepAway()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
        }
    }

    private void StartObstacleCourse()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
        }
    }

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
        // Disable player controls and show game over screen
        player.SetActive(false);
    }
}
