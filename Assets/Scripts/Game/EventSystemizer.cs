using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.EventSystems;

namespace Game
{
    /// <summary>
    /// This class makes sure there is only one EventSystem in the game at any time.
    /// </summary>
    public class EventSystemizer : MonoBehaviour
    {
        /// <summary>
        /// Checks every frame to ensure there is only one EventSystem in the game.
        /// </summary>
        private void Update()
        {
            // Find all EventSystem objects in the scene
            foreach (EventSystem system in FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
            {
                // Skip the EventSystem attached to this GameObject
                if (system == GetComponent<EventSystem>()) continue;

                // Remove any extra EventSystem objects
                Destroy(system.gameObject);
            }
        }
    }
}