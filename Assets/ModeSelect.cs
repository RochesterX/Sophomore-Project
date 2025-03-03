using UnityEngine;
using UnityEngine.UI;

public class ModeSelect : MonoBehaviour
{
    private ToggleGroup maps;

    private void Start()
    {
        maps = GetComponent<ToggleGroup>();
    }

    void Update()
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
        else if (toggle.name == "Obstacle-Course")
        {
            GameManager.gameMode = GameManager.GameMode.obstacleCourse;
        }
    }
}
