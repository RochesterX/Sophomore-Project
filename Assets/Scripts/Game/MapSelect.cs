using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// This class manages the map selection process by setting the selected map
    /// based on the active toggle in the toggle group.
    /// </summary>
    public class MapSelect : MonoBehaviour
    {
        /// <summary>
        /// The toggle group containing the map selection toggles.
        /// </summary>
        private ToggleGroup maps;

        /// <summary>
        /// Initializes the toggle group for map selection.
        /// </summary>
        private void Start()
        {
            maps = GetComponent<ToggleGroup>();
        }

        /// <summary>
        /// Updates the selected map in the game manager based on the active toggle.
        /// </summary>
        private void Update()
        {
            // Get the currently active toggle and set the selected map
            Toggle toggle = maps.GetFirstActiveToggle();
            GameManager.map = toggle.name;
        }
    }
}