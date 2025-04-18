using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// This class manages the display of player lives, including creating life icons
    /// and updating them based on the player's remaining lives.
    /// </summary>
    public class LifeDisplayManager : MonoBehaviour
    {
        /// <summary>
        /// The parent object that contains all player life displays.
        /// </summary>
        public GameObject players;

        /// <summary>
        /// The prefab used to represent a player in the life display.
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// The prefab used to represent a single life icon.
        /// </summary>
        public GameObject lifePrefab;

        /// <summary>
        /// A dictionary mapping each player's <see cref="Damageable"/> component
        /// to their corresponding list of life icons.
        /// </summary>
        public Dictionary<Damageable, List<GameObject>> lifeDisplays = new Dictionary<Damageable, List<GameObject>>();

        /// <summary>
        /// Initializes the life display by creating life icons for each player.
        /// </summary>
        private void Start()
        {
            // Only initialize life displays in free-for-all game mode
            if (GameManager.gameMode == GameManager.GameMode.freeForAll)
            {
                foreach (GameObject player in GameManager.players)
                {
                    // Create a parent object for the player's life icons
                    Transform parent = Instantiate(playerPrefab, players.transform).transform;

                    // Create life icons based on the player's number of lives
                    List<GameObject> lives = new List<GameObject>();
                    for (int i = 0; i < player.GetComponent<Damageable>().lives; i++)
                    {
                        GameObject life = Instantiate(lifePrefab, parent);

                        // Set the color of the life icon to match the player's color
                        life.transform.Find("LIFE").GetComponent<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
                        lives.Add(life);
                    }

                    // Map the player's Damageable component to their life icons
                    lifeDisplays.Add(player.GetComponent<Damageable>(), lives);
                }
            }
        }

        /// <summary>
        /// Updates the life display to reflect the current number of lives for each player.
        /// </summary>
        private void Update()
        {
            foreach (Damageable damageable in lifeDisplays.Keys)
            {
                // Enable or disable life icons based on the player's remaining lives
                foreach (GameObject life in lifeDisplays[damageable])
                {
                    life.SetActive(lifeDisplays[damageable].IndexOf(life) < damageable.lives);
                }
            }
        }

        /// <summary>
        /// Hides the life display by deactivating the parent object.
        /// </summary>
        public void HideLifeDisplay()
        {
            players.SetActive(false);
        }
    }
}