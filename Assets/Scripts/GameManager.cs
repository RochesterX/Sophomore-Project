using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        freeForAll,
        teamDeathmatch,
        captureTheFlag
    }

    public static GameMode gameMode = GameMode.freeForAll;
}
