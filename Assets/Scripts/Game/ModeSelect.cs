using UnityEngine; using Game; using Music; using Player;
using UnityEngine.UI;
namespace Game
{

public class ModeSelect : MonoBehaviour
{
    private ToggleGroup maps;

    private void Start()
    {
        maps = GetComponent<ToggleGroup>();
    }

    void Update() // Updates the game mode based on the selected toggle
    {
        Toggle toggle = maps.GetFirstActiveToggle();
        if (toggle.name == "Free-For-All")
        {
            GameManager.gameMode = GameManager.GameMode.freeForAll;
        }
        else if (toggle.name == "Keep-Away")
        {
            GameManager.gameMode = GameManager.GameMode.keepAway;
        }
        else if (toggle.name == "Obstacle Course")
        {
            GameManager.gameMode = GameManager.GameMode.obstacleCourse;
        }
    }
}
}