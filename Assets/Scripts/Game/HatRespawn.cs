using UnityEngine;
using Game;
using Music;
using Player;
using System.Collections;

namespace Game
{
    /// <summary>
    /// This class manages the behavior of the hat in the game, including its respawn logic.
    /// The hat can be picked up, dropped, and respawned after a certain amount of time.
    /// </summary>
    public class HatRespawn : MonoBehaviour
    {
        /// <summary>
        /// The last time the hat was interacted with.
        /// </summary>
        private float lastInteractionTime;

        /// <summary>
        /// The amount of time (in seconds) before the hat respawns after being inactive.
        /// </summary>
        public const float respawnTime = 10f;

        /// <summary>
        /// Indicates whether the hat is currently dropped.
        /// </summary>
        private bool isDropped;

        /// <summary>
        /// The initial position of the subhat (if applicable).
        /// </summary>
        public Vector2 initialSubhatPosition;

        /// <summary>
        /// A flag to check if the hat can be picked up.
        /// </summary>
        public static bool canBePickedUp = true;

        /// <summary>
        /// The initial scale of the hat.
        /// </summary>
        public Vector2 initialScale;

        /// <summary>
        /// Saves the initial scale of the hat when the game starts.
        /// </summary>
        private void Awake()
        {
            initialScale = transform.lossyScale;
        }

        /// <summary>
        /// Sets up the hat's initial position and state when the game starts.
        /// </summary>
        private void Start()
        {
            lastInteractionTime = Time.time;
            isDropped = false;

            // Place the hat at a random spawn position
            transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
        }

        /// <summary>
        /// Checks if the hat has been inactive for too long and respawns it if necessary.
        /// </summary>
        private void Update()
        {
            // Disable the hat's collider if the game is over
            if (GameManager.Instance.gameOver)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }

            // Respawn the hat if it has been dropped for too long
            if (isDropped && Time.time - lastInteractionTime > respawnTime)
            {
                StartCoroutine(RespawnHat());
            }
        }

        /// <summary>
        /// Respawns the hat if it falls out of bounds.
        /// </summary>
        /// <param name="collision">The object that collided with the hat.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Platformer Hazard"))
            {
                isDropped = true;
                StartCoroutine(RespawnHat());
            }
        }

        /// <summary>
        /// Updates the last interaction time when a player interacts with the hat.
        /// </summary>
        public void Interact()
        {
            lastInteractionTime = Time.time;
            isDropped = false;
        }

        /// <summary>
        /// Marks the hat as dropped and resets the timer.
        /// </summary>
        public void OnHatDropped()
        {
            lastInteractionTime = Time.time;
            isDropped = true;
        }

        /// <summary>
        /// Respawns the hat at a random spawn position after a short delay.
        /// </summary>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator RespawnHat()
        {
            lastInteractionTime = Time.time;
            // Play the respawn animation
            GetComponentInChildren<Animator>().SetTrigger("respawn");

            // Wait briefly before respawning the hat
            yield return new WaitForSeconds(1f / 3f / 2f);

            // Move the hat to a random spawn position and reset its state
            transform.position = GameManager.Instance.hatSpawnPositions[Random.Range(0, GameManager.Instance.hatSpawnPositions.Count - 1)];
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
            isDropped = false;
        }
    }
}



