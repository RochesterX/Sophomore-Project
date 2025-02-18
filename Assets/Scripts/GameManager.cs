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
}
