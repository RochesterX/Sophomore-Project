using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class helps manage positions for the GameManager during development.
    /// It allows adding hat spawn positions and player spawn positions directly in the editor.
    /// </summary>
    [ExecuteAlways]
    public class GameManagerHelper : MonoBehaviour
    {
        /// <summary>
        /// If true, adds the current position of the "HELPER" object to the hat spawn positions in the GameManager.
        /// </summary>
        public bool addHatPosition;

        /// <summary>
        /// If true, sets the current position of the "HELPER" object as the player spawn position in the GameManager.
        /// </summary>
        public bool addSpawnPosition;

        /// <summary>
        /// Checks for changes to the addHatPosition and addSpawnPosition flags every frame.
        /// Updates the GameManager with the corresponding positions when the flags are set.
        /// </summary>
        private void Update()
        {
            // Add the current position of the "HELPER" object to the hat spawn positions
            if (addHatPosition)
            {
                addHatPosition = false; // Reset the flag
                GetComponent<GameManager>().hatSpawnPositions.Add(GameObject.Find("HELPER").transform.position);
            }

            // Set the current position of the "HELPER" object as the player spawn position
            if (addSpawnPosition)
            {
                addSpawnPosition = false; // Reset the flag
                GetComponent<GameManager>().spawnPosition = GameObject.Find("HELPER").transform.position;
            }
        }
    }
}