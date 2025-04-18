using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class handles respawning objects when they collide with a trigger tagged with a specific value.
    /// </summary>
    public class RespawnOnTriggerEnter : MonoBehaviour
    {
        /// <summary>
        /// The spawn point where the object will respawn.
        /// </summary>
        public Vector2 spawnPoint;

        /// <summary>
        /// If true, the spawn point is set to the object's initial position.
        /// </summary>
        public bool spawnPointIsInitialPosition = false;

        /// <summary>
        /// The tag of the trigger that causes the object to respawn.
        /// </summary>
        public string respawnTag;

        /// <summary>
        /// Sets the spawn point to the object's initial position if specified.
        /// </summary>
        private void Start()
        {
            if (spawnPointIsInitialPosition)
            {
                // Set the spawn point to the object's initial position
                spawnPoint = transform.position;
            }
        }

        /// <summary>
        /// Handles collisions with triggers and applies damage to the object if it has a <see cref="Damageable"/> component.
        /// </summary>
        /// <param name="other">The collider of the object that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the collider has the specified tag
            if (other.CompareTag(respawnTag))
            {
                // Apply damage to the object if it has a Damageable component
                if (TryGetComponent(out Damageable damageable))
                {
                    damageable.Damage(9999f);
                }
            }
        }
    }
}