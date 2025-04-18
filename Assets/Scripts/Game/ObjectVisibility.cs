using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class controls the visibility of an object based on the current game mode.
    /// </summary>
    public class ObjectVisibility : MonoBehaviour
    {
        /// <summary>
        /// Initializes the visibility of the object when the game starts.
        /// </summary>
        private void Start()
        {
            UpdateVisibility();
        }

        /// <summary>
        /// Updates the visibility of the object every frame.
        /// </summary>
        private void Update()
        {
            UpdateVisibility();
        }

        /// <summary>
        /// Sets the object to be visible only if the game mode is "Keep Away".
        /// </summary>
        private void UpdateVisibility()
        {
            if (GameManager.gameMode == GameManager.GameMode.keepAway)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}