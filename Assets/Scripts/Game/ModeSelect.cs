using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// This class manages the game mode selection process by setting the game mode
    /// based on the active toggle in the toggle group.
    /// </summary>
    public class ModeSelect : MonoBehaviour
    {
        /// <summary>
        /// The toggle group containing the game mode selection toggles.
        /// </summary>
        private ToggleGroup maps;

        /// <summary>
        /// Initializes the toggle group for game mode selection.
        /// </summary>
        private void Start()
        {
            maps = GetComponent<ToggleGroup>();
        }

        /// <summary>
        /// Updates the selected game mode in the game manager based on the active toggle.
        /// </summary>
        private void Update()
        {
            // Get the currently active toggle and set the game mode
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