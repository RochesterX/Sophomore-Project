using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

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
}
