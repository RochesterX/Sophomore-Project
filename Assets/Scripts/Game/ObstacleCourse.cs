using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class handles the logic for detecting when a player completes the obstacle course.
    /// </summary>
    public class ObstacleCourse : MonoBehaviour
    {
        /// <summary>
        /// The player who successfully completes the obstacle course.
        /// </summary>
        public static GameObject playerWon;

        /// <summary>
        /// Detects when a player enters the trigger area and ends the game.
        /// </summary>
        /// <param name="collision">The collider of the object that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the object entering the trigger is a player
            if (collision.gameObject.CompareTag("Player"))
            {
                // Set the player who won and trigger the game over logic
                playerWon = collision.gameObject;
                GameManager.Instance.GameOver();
            }
        }
    }
}