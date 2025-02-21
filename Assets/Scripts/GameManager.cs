<<<<<<< Updated upstream
using System.Collections.Generic;
=======
using Unity.VisualScripting;
>>>>>>> Stashed changes
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  startGame
    { 
        if GameMode = GameMode.freeForAll(
        Start freeForAll)
        if GameMode = GameMode.freeForAll(
        Start freeForAll)
        if GameMode = GameMode.freeForAll(
        Start freeForAll);
    }

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
