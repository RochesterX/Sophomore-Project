using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Player
{
    /// <summary>
    /// This class handles the player's ability to take damage, die, and respawn.
    /// It also manages interactions like blocking, parrying, and dropping items when hit.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(RespawnOnTriggerEnter))]
    public class Damageable : MonoBehaviour
    {
        /// <summary>
        /// The force applied to the player when hit.
        /// </summary>
        public float force = 50f;

        /// <summary>
        /// The current accumulated damage of the player.
        /// </summary>
        public float damage = 0f;

        /// <summary>
        /// The maximum damage the player can take before dying.
        /// </summary>
        public float maxDamage = 1000f;

        /// <summary>
        /// The number of lives the player has.
        /// </summary>
        public int lives = 0;

        /// <summary>
        /// If true, applies damage to self for debugging purposes.
        /// </summary>
        public bool damageSelfDebug = false;

        /// <summary>
        /// Indicates whether the player is currently dying.
        /// </summary>
        public bool dying = false;

        /// <summary>
        /// Event triggered when a player dies.
        /// </summary>
        public event System.Action<GameObject> OnPlayerDeath;

        /// <summary>
        /// Event triggered when a player respawns.
        /// </summary>
        public event System.Action<GameObject> OnPlayerRespawn;

        /// <summary>
        /// Reference to the player's animator component.
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Initializes the animator reference.
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Handles debug self-damage if enabled.
        /// </summary>
        private void Update()
        {
            if (damageSelfDebug)
            {
                damageSelfDebug = false;
                Damage(gameObject);
            }
        }

        /// <summary>
        /// Applies damage when colliding with a punch hurtbox.
        /// </summary>
        /// <param name="collision">The collider that entered the trigger.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Punch Hurtbox"))
            {
                Damage(collision.transform.parent.gameObject);
            }
        }

        /// <summary>
        /// Applies damage to the player from a given damage source.
        /// Handles blocking, parrying, and force application.
        /// </summary>
        /// <param name="damageSource">The GameObject causing the damage.</param>
        private void Damage(GameObject damageSource)
        {
            // Prevent damage if the player is dying or the damage source is a hat
            if (dying || damageSource.CompareTag("Hat")) return;

            float actualForce = damageSource.GetComponent<Damageable>().force;
            Block blockComponent = GetComponent<Block>();

            // Drop the item if the player is holding one
            GetComponentInChildren<UseItem>().DropItem();

            if (blockComponent != null && blockComponent.blocking)
            {
                if (blockComponent.IsParrying())
                {
                    // Handle parry logic
                    damageSource.GetComponent<Damageable>().SuccessfulParry(gameObject, actualForce);
                    AudioManager.Instance.PlaySound("Parry");
                    return;
                }
                else
                {
                    // Reduce damage if the player is blocking
                    AudioManager.Instance.PlaySound("Punch");
                    actualForce /= 4;
                    GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce, ForceMode2D.Force);
                }
            }
            else
            {
                // Apply full damage if the player is not blocking
                AudioManager.Instance.PlaySound("Punch");
                GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce * (1 + (damage / maxDamage) * 3), ForceMode2D.Force);
            }

            // Update the player's damage and check if they should die
            damage += actualForce;
            damage = Mathf.Clamp(damage, 0f, maxDamage);
            if (damage >= maxDamage)
            {
                Die();
            }
        }

        /// <summary>
        /// Adds a specified amount of damage to the player.
        /// </summary>
        /// <param name="damage">The amount of damage to add.</param>
        public void Damage(float damage)
        {
            this.damage += damage;
            if (this.damage >= maxDamage)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles the effects of a successful parry, applying force and damage.
        /// </summary>
        /// <param name="damageSource">The GameObject that was parried.</param>
        /// <param name="force">The force to apply.</param>
        private void SuccessfulParry(GameObject damageSource, float force)
        {
            GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * force, ForceMode2D.Force);
            damage += force;
            damage = Mathf.Clamp(damage, 0f, maxDamage);

            if (damage >= maxDamage)
            {
                Die();
            }
        }

        /// <summary>
        /// Triggers the death animation and sets the player to the dying state.
        /// </summary>
        private void Die()
        {
            if (GameManager.Instance != null)
            {
                // Drop the item if the player is holding one
                UseItem useItem = GetComponent<UseItem>();
                if (useItem != null)
                {
                    useItem.DropItem();
                }

                // Trigger the death animation and mark the player as dying
                animator.SetBool("die", true);
                dying = true;

                AudioManager.Instance.PlaySound("Death Simple");
                OnPlayerDeath?.Invoke(gameObject);
            }
        }

        /// <summary>
        /// Handles player state after death and resets dying state after respawn.
        /// </summary>
        public void HandleDeath()
        {
            print("Player " + gameObject.name + " died");
            GameManager.Instance.PlayerDied(this);
            animator.SetBool("die", false);
            dying = false;
        }

        /// <summary>
        /// Respawns the player at the spawn position and resets damage/health bar.
        /// </summary>
        public void Respawn()
        {
            //transform.position = GameManager.Instance.spawnPosition;

            // Reset the player's velocity
            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Reset the player's damage
            ResetDamage();

            OnPlayerRespawn?.Invoke(gameObject);
        }

        /// <summary>
        /// Resets the player's damage to zero.
        /// </summary>
        public void ResetDamage()
        {
            damage = 0f;
        }
    }
}