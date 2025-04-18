using UnityEngine;
using Game;
using Music;
using Player;

namespace Player
{
    /// <summary>
    /// This class handles teleportation for platforms and players when they collide with the teleport trigger.
    /// It can teleport either the platform itself or a player to a specified location.
    /// </summary>
    public class TeleportPlatform : MonoBehaviour
    {
        /// <summary>
        /// The position where the platform or player will be teleported.
        /// </summary>
        public Vector2 teleportPoint;

        /// <summary>
        /// The tag used to identify objects (e.g., platforms) that can trigger teleportation.
        /// </summary>
        public string teleportTag;

        /// <summary>
        /// The tag used to identify player objects.
        /// </summary>
        public string playerTag = "Player";

        /// <summary>
        /// Determines whether this script is handling a platform or a player.
        /// If true, it teleports players. If false, it teleports the platform itself.
        /// </summary>
        public bool isPlatform = true;

        /// <summary>
        /// Handles the teleportation logic when an object enters the trigger collider.
        /// </summary>
        /// <param name="collision">The collider of the object that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isPlatform)
            {
                // Teleports the platform itself
                if (collision.CompareTag(teleportTag))
                {
                    // Move the platform to the teleport point
                    transform.position = teleportPoint;

                    // Reset the platform's velocity if it has a Rigidbody2D component
                    if (TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }
            else
            {
                // Teleports the player
                if (collision.CompareTag(playerTag))
                {
                    // Move the player to the teleport point
                    collision.transform.position = teleportPoint;

                    // Reset the player's velocity if they have a Rigidbody2D component
                    if (collision.TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }
        }
    }
}
