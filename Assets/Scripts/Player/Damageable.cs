using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
namespace Player
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(RespawnOnTriggerEnter))]
    public class Damageable : MonoBehaviour
    {

        /// <summary>
        /// The force applied to the player when hit.
        /// </summary>
        public float force = 50f; // Force applied when hit

        /// <summary>
        /// The current accumulated damage of the player.
        /// </summary>
        public float damage = 0f;

        /// <summary>
        /// The maximum damage the player can take before dying.
        /// </summary>
        public float maxDamage = 1000f; // Set max health

        /// <summary>
        /// The number of lives the player has.
        /// </summary>
        public int lives = 0;

        private Animator animator;

        /// <summary>
        /// If true, applies damage to self for debugging purposes.
        /// </summary>
        public bool damageSelfDebug = false;

        /// <summary>
        /// Indicates whether the player is currently dying.
        /// </summary>
        public bool dying = false;

        /// <summary>
        /// Event triggered when the player is punched.
        /// </summary>
        public event System.Action<GameObject> OnPlayerPunched;

        /// <summary>
        /// Unity Start method. Initializes the animator reference.
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Unity Update method. Handles debug self-damage if enabled.
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
        /// Unity OnTriggerEnter2D method. Applies damage when colliding with a punch hurtbox.
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
            if (dying || damageSource.CompareTag("Hat")) return; // Exclude hat from taking damage

            float actualForce = damageSource.GetComponent<Damageable>().force;
            Block blockComponent = GetComponent<Block>();

            GetComponentInChildren<UseItem>().DropItem(); // Drops hat if held

            if (blockComponent != null && blockComponent.blocking)
            {
                if (blockComponent.IsParrying()) // Player receives damage if punching a parrying player
                {
                    damageSource.GetComponent<Damageable>().SuccessfulParry(gameObject, actualForce);
                    AudioManager.Instance.PlaySound("Parry");
                    return;
                }
                else // Player does less damage if punching a blocking player
                {
                    AudioManager.Instance.PlaySound("Punch");
                    actualForce /= 4;
                    GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce, ForceMode2D.Force);
                }
            }
            else // Player does full damage to a non-blocking player
            {
                AudioManager.Instance.PlaySound("Punch");
                GetComponent<Rigidbody2D>().AddForce(((transform.position - damageSource.transform.position).normalized + Vector3.up * 2) * actualForce * (1 + (damage / maxDamage) * 3), ForceMode2D.Force);
            }
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
            //if (GameManager.Instance.gameOver) return; // Prevent damage after game is over

            this.damage += damage;
            if (damage >= maxDamage)
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
            if (GameManager.Instance != null) //&& !GameManager.Instance.gameOver) // Prevent death after game is over
            {
                UseItem useItem = GetComponent<UseItem>();
                if (useItem != null)
                {
                    useItem.DropItem(); // Ensure the player drops the item before the death animation
                }
                animator.SetBool("die", true);
                dying = true;

                AudioManager.Instance.PlaySound("Death Simple");
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
            transform.position = GameManager.Instance.spawnPosition;
            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            if (TryGetComponent<Damageable>(out var damageable))
            {
                damageable.ResetDamage();
            }
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