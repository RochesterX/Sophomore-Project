using System.Collections;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class controls platforms that fall when touched by a player or another platform.
    /// The platform will fall after a delay and then reset to its original position.
    /// </summary>
    public class FallPlatform : MonoBehaviour
    {
        /// <summary>
        /// The time (in seconds) before the platform starts falling after being triggered.
        /// </summary>
        public float fallDelay = 2f;

        /// <summary>
        /// The time (in seconds) before the platform resets to its original position after falling.
        /// </summary>
        public float resetDelay = 4f;

        /// <summary>
        /// Indicates whether the platform is currently falling.
        /// </summary>
        private bool falling;

        /// <summary>
        /// Reference to the Rigidbody2D component of the platform's parent object.
        /// </summary>
        private Rigidbody2D rb;

        /// <summary>
        /// The original position of the platform's parent object.
        /// </summary>
        private Vector3 defposition;

        /// <summary>
        /// Initializes the platform's Rigidbody2D and stores its original position.
        /// </summary>
        private void Start()
        {
            defposition = transform.parent.position;
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Triggers the platform to fall when a player or another platform touches it.
        /// </summary>
        /// <param name="collision">The object that collided with the platform.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            try
            {
                // Check if the collision is caused by a player or another falling platform
                if (collision.transform.childCount != 0 && !falling &&
                    (collision.gameObject.CompareTag("Player") || collision.transform.GetChild(0).TryGetComponent(out FallPlatform _)))
                {
                    StartCoroutine(FallAfterDelay());
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error in FallPlatform: " + e.Message);
            }
        }

        /// <summary>
        /// Makes the platform fall after a delay and resets it after another delay.
        /// </summary>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator FallAfterDelay()
        {
            falling = true;

            // Wait for the fall delay before making the platform fall
            yield return new WaitForSeconds(fallDelay);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.angularVelocity = Random.Range(-30, 30);

            // Wait for the reset delay before resetting the platform
            yield return new WaitForSeconds(resetDelay);
            transform.parent.GetComponent<Animator>().SetTrigger("respawn");

            // Wait briefly before resetting the platform
            yield return new WaitForSeconds(0.5f);
            Respawn();
        }

        /// <summary>
        /// Resets the platform to its original position and state.
        /// </summary>
        private void Respawn()
        {
            falling = false;

            // Set the platform back to a static state
            rb.bodyType = RigidbodyType2D.Static;

            // Reset the platform's position and rotation
            transform.parent.position = defposition;
            transform.parent.rotation = Quaternion.identity;
        }
    }
}


