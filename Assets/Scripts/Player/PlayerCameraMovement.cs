using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Player
{
    /// <summary>
    /// This class controls the movement of the camera to follow players during the game.
    /// It also handles special behavior for the camera in the win scene.
    /// </summary>
    public class PlayerCameraMovement : MonoBehaviour
    {
        /// <summary>
        /// The starting position of the camera.
        /// </summary>
        private Vector3 start;

        /// <summary>
        /// The target position the camera should move toward.
        /// </summary>
        private Vector3 target;

        /// <summary>
        /// The weight used to blend between the camera's starting position and the players' average position.
        /// </summary>
        public float weight;

        /// <summary>
        /// The speed at which the camera moves toward the target position.
        /// </summary>
        public float speed;

        /// <summary>
        /// The player who won the game, used to focus the camera in the win scene.
        /// </summary>
        private GameObject playerThatWon;

        /// <summary>
        /// The lowest vertical position the camera can move to.
        /// </summary>
        public float lowerBound;

        /// <summary>
        /// Indicates whether the camera is in the win scene mode.
        /// </summary>
        public bool winScene = false;

        /// <summary>
        /// Indicates whether the camera should remain static and not follow players.
        /// </summary>
        public bool staticCamera = false;

        /// <summary>
        /// Initializes the camera's starting position.
        /// </summary>
        private void Start()
        {
            start = transform.position;
        }

        /// <summary>
        /// Updates the camera's position every frame to follow players or focus on the winner.
        /// </summary>
        private void Update()
        {
            // If the game is over, focus the camera on the player that won
            if (winScene)
            {
                if (playerThatWon == null || !playerThatWon.activeInHierarchy)
                {
                    playerThatWon = FindWinner();
                }

                if (playerThatWon != null)
                {
                    // Move the camera toward the winning player's position
                    target = playerThatWon.transform.position;
                    transform.position = Vector3.Lerp(
                        transform.position,
                        new Vector3(target.x, target.y, target.z - 10),
                        speed * 12 * Time.deltaTime
                    );

                    // Ensure the camera does not go below the lower bound
                    if (transform.position.y < lowerBound)
                    {
                        transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
                    }
                }

                return;
            }

            // Follow the players during the game
            List<GameObject> players = GameManager.players;
            if (players.Count == 0) return;

            Vector3 playerAverage = Vector3.zero;
            int activePlayers = 0;

            // Calculate the average position of all active players
            foreach (GameObject player in players)
            {
                if (player == null || !player.activeInHierarchy) continue;

                Damageable damageable = player.GetComponent<Damageable>();
                if (damageable != null && damageable.dying) continue;

                playerAverage += player.transform.position;
                activePlayers++;
            }

            if (activePlayers == 0 || staticCamera) return;

            playerAverage /= activePlayers;

            // Blend between the starting position and the players' average position
            target = start * weight + playerAverage * (1 - weight);
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(target.x, target.y, transform.position.z),
                speed * Time.deltaTime
            );

            // Ensure the camera does not go below the lower bound
            if (transform.position.y < lowerBound)
            {
                transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
            }
        }

        /// <summary>
        /// Activates the win scene mode and focuses the camera on the winning player.
        /// </summary>
        /// <param name="player">The player who won the game.</param>
        public void WinScene(GameObject player)
        {
            winScene = true;
            playerThatWon = player;
        }

        /// <summary>
        /// Finds the first active player in the game, used to determine the winner.
        /// </summary>
        /// <returns>The first active player GameObject, or null if no players are active.</returns>
        private GameObject FindWinner()
        {
            foreach (GameObject player in GameManager.players)
            {
                if (player != null && player.activeInHierarchy)
                {
                    return player;
                }
            }
            return null;
        }
    }
}

