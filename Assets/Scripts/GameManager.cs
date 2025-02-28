using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    public static GameMode gameMode = GameMode.freeForAll;
    public static string map = "Platformer With Headroom";
    public static List<GameObject> players = new List<GameObject>();
    public Vector2 spawnPosition;

    private Dictionary<GameObject, int> playerLives = new Dictionary<GameObject, int>();
    public int maxLives = 3;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        foreach (GameObject player in players)
        {
            if (gameMode == GameMode.freeForAll)
            {
                playerLives[player] = maxLives;
            }
            else
            {
                playerLives[player] = 1;
            }
            player.transform.position = spawnPosition;
        }
    }

    public void PlayerDied(GameObject player)
    {
        if (gameMode == GameMode.freeForAll)
        {
            playerLives[player]--;
            if (playerLives[player] <= 0)
            {
                GameOver(player);
            }
            else
            {
                RespawnPlayer(player);
            }
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
