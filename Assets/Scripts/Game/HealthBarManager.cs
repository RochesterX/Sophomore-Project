using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class manages the health bars for all players in the game.
    /// It creates, updates, and removes health bars as needed.
    /// </summary>
    public class HealthBarManager : MonoBehaviour
    {
        /// <summary>
        /// The template used to create new health bars.
        /// </summary>
        public GameObject healthBarPrefab;

        /// <summary>
        /// A dictionary that links each player to their health bar.
        /// </summary>
        private Dictionary<GameObject, GameObject> playerHealthBars = new Dictionary<GameObject, GameObject>();

        /// <summary>
        /// Sets up event listeners for when the game starts and ends.
        /// </summary>
        private void Awake()
        {
            print("Doing event stuff");
            GameManager.Instance.StartGameEvent += OnGameStart;
            GameManager.Instance.EndGameEvent += OnGameEnd;
            print("Done event stuff");
        }

        /// <summary>
        /// Removes event listeners when this object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            GameManager.Instance.StartGameEvent -= OnGameStart;
            GameManager.Instance.EndGameEvent -= OnGameEnd;
        }

        /// <summary>
        /// Updates the position of each health bar to follow its player.
        /// </summary>
        private void Update()
        {
            foreach (var kvp in playerHealthBars)
            {
                GameObject player = kvp.Key;
                if (player == null) continue;

                GameObject healthBar = kvp.Value;

                // Position the health bar slightly above the player
                healthBar.transform.SetPositionAndRotation(
                    new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z),
                    Quaternion.identity
                );
            }
        }

        /// <summary>
        /// Creates health bars for all players when the game starts.
        /// </summary>
        private void OnGameStart()
        {
            print("Ongame start called");
            foreach (GameObject player in GameManager.players)
            {
                if (!playerHealthBars.ContainsKey(player))
                {
                    CreateHealthBar(player);

                    // Listen for the player's death and respawn events
                    var damageable = player.GetComponent<Damageable>();
                    damageable.OnPlayerDeath += HandlePlayerDeath;
                    damageable.OnPlayerRespawn += HandlePlayerRespawn;
                }
            }
        }

        /// <summary>
        /// Creates a health bar for a specific player.
        /// </summary>
        /// <param name="player">The player to create a health bar for.</param>
        private void CreateHealthBar(GameObject player)
        {
            // Create a new health bar and link it to the player
            GameObject healthBar = Instantiate(healthBarPrefab);
            healthBar.transform.localScale *= 1.5f; // Make the health bar slightly larger
            healthBar.GetComponent<TerribleHealthBarScript>().SetPlayer(player);
            playerHealthBars[player] = healthBar;
        }

        /// <summary>
        /// Handles the player's respawn by recreating their health bar if needed.
        /// </summary>
        /// <param name="player">The player who respawned.</param>
        private void HandlePlayerRespawn(GameObject player)
        {
            if (!playerHealthBars.ContainsKey(player))
            {
                CreateHealthBar(player);
            }
        }

        /// <summary>
        /// Handles the player's death by removing their health bar.
        /// </summary>
        /// <param name="player">The player who died.</param>
        private void HandlePlayerDeath(GameObject player)
        {
            if (playerHealthBars.TryGetValue(player, out GameObject healthBar))
            {
                Destroy(healthBar);
                playerHealthBars.Remove(player);
            }
        }

        /// <summary>
        /// Cleans up all health bars and unsubscribes from player events when the game ends.
        /// </summary>
        private void OnGameEnd()
        {
            print("Ongame end called");
            // Remove all health bars
            foreach (var kvp in playerHealthBars)
            {
                Destroy(kvp.Value);
            }
            playerHealthBars.Clear();

            // Unsubscribe from all player events
            foreach (GameObject player in GameManager.players)
            {
                if (player != null && player.TryGetComponent<Damageable>(out var damageable))
                {
                    damageable.OnPlayerDeath -= HandlePlayerDeath;
                    damageable.OnPlayerRespawn -= HandlePlayerRespawn;
                }
            }
        }
    }
}