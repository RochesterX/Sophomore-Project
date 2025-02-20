using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
        }
    }
}
