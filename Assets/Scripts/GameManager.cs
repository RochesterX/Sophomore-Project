using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
=======

    public void StartGame()
    {
        if (gameMode == GameMode.freeForAll)
        {
            StartFreeForAll();
        }
        if (gameMode == GameMode.keepAway)
        {
            StartKeepAway();
        }
        if (gameMode == GameMode.obstacleCourse)
        {
            StartObstacleCourse();
        }
    }

>>>>>>> Stashed changes
    public enum GameMode
    {
        freeForAll,
        keepAway,
        obstacleCourse
    }

    public static GameMode gameMode = GameMode.freeForAll;

    public static string map = "Platformer With Headroom"; //is called for in playermanager but should probably be removed.

    public static List<GameObject> players = new List<GameObject>();

    public Vector2 spawnPosition;

    private void StartFreeForAll()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
        }
    }

<<<<<<< Updated upstream
    public void StartGame()
    {
        if (gameMode == GameMode.freeForAll)
        {
            // Start free for all game
        }
    }
=======
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

    private void Start()
    {
        StartGame();
    }
>>>>>>> Stashed changes
}
