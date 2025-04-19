using System.Collections;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Player
{
    /// <summary>
    /// This class allows a player to pick up, hold, and drop items during the game.
    /// It is primarily used for managing interactions with the "hat" in "keep-away" mode.
    /// </summary>
    public class UseItem : MonoBehaviour
    {
        /// <summary>
        /// The tag used to identify items that can be picked up.
        /// </summary>
        [SerializeField] private string itemTag;

        /// <summary>
        /// The item currently being held by the player.
        /// </summary>
        private GameObject heldItem;

        /// <summary>
        /// Whether the player is currently holding an item.
        /// </summary>
        private bool isHoldingItem = false;

        /// <summary>
        /// The time when the player started holding the item.
        /// </summary>
        private float holdStartTime;

        /// <summary>
        /// The total time the player has held the item.
        /// </summary>
        public float holdTime;

        /// <summary>
        /// Reference to the player's <see cref="Damageable"/> component.
        /// </summary>
        private Damageable damageable;

        /// <summary>
        /// The position where the item will be held (e.g., above the player's head).
        /// </summary>
        [SerializeField] public Transform head;

        /// <summary>
        /// Initializes the player's <see cref="Damageable"/> component.
        /// </summary>
        private void Start()
        {
            damageable = GetComponent<Damageable>();
        }

        /// <summary>
        /// Updates the player's state every frame.
        /// If the player is holding an item, it keeps the item positioned on their head
        /// and updates the hold time in "keep-away" mode.
        /// </summary>
        private void Update()
        {
            if (isHoldingItem)
            {
                // Keeps the item positioned on the player's head
                heldItem.transform.localPosition = Vector3.zero;

                if (GameManager.gameMode == GameManager.GameMode.keepAway)
                {
                    // Adds time to the player's leaderboard standing
                    holdTime += Time.deltaTime;
                    GameManager.Instance.UpdatePlayerHoldTime(gameObject, holdTime);
                }
            }
        }

        /// <summary>
        /// Automatically picks up an item when the player collides with it.
        /// </summary>
        /// <param name="collision">The collision data from the item.</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the collided object is a "hat" and the player is not already holding an item
            if (collision.gameObject.CompareTag(itemTag) && !isHoldingItem && !damageable.dying)
            {
                PickUpItem(collision.gameObject);
            }
        }

        /// <summary>
        /// Automatically picks up an item when the player enters its triggwer.
        /// </summary>
        /// <param name="collision">The collision data from the item.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the collided object is a "hat" and the player is not already holding an item
            if (collision.gameObject.CompareTag(itemTag) && !isHoldingItem && !damageable.dying)
            {
                PickUpItem(collision.gameObject);
            }
        }

        /// <summary>
        /// Allows the player to pick up an item and start the hold timer.
        /// </summary>
        /// <param name="item">The item to pick up.</param>
        public void PickUpItem(GameObject item)
        {
            // Prevent picking up items if the player is dying or the item is not interactable
            if (damageable.dying) return;
            if (HatRespawn.canBePickedUp == false) return;

            // Set the item as the held item and update its state
            heldItem = item;
            isHoldingItem = true;
            holdStartTime = Time.time;
            heldItem.GetComponent<Collider2D>().enabled = false;
            heldItem.transform.Find("HatPhysical").GetComponent<Collider2D>().enabled = false;
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            item.GetComponent<HatRespawn>().Interact();
            item.transform.parent = head;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = Vector3.zero;

            // Initialize the player's hold time in the GameManager if not already set
            if (!GameManager.playerHoldTimes.ContainsKey(gameObject))
            {
                GameManager.playerHoldTimes[gameObject] = 0f;
            }

            // Play the pickup sound and stop any ongoing hat movement
            AudioManager.Instance.PlaySound("Pickup Hat");
            GameManager.Instance.StopCoroutine("MoveHatToWinner");
        }

        /// <summary>
        /// Allows the player to drop the item they are holding.
        /// </summary>
        public void DropItem()
        {
            // Prevent dropping items if the game is over
            if (GameManager.Instance.gameOver) return;

            if (isHoldingItem)
            {
                // Enable the item's collider and make it interactable after a short delay
                heldItem.GetComponent<Collider2D>().enabled = true;
                heldItem.transform.Find("HatPhysical").GetComponent<Collider2D>().enabled = true;
                HatRespawn.canBePickedUp = false;
                StartCoroutine(WaitForInteractability());

                // Make the item dynamic and apply random force and torque to it
                heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                heldItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(10f, 30f) + Vector2.right * Random.Range(-10, 10), ForceMode2D.Impulse);
                heldItem.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-5, 5), ForceMode2D.Impulse);

                // Notify the item that it has been dropped
                heldItem.GetComponent<HatRespawn>().OnHatDropped();

                // Detach the item from the player
                heldItem.transform.parent = GameManager.Instance.transform;
                heldItem = null;
                isHoldingItem = false;

                // Remove the player's hold time from the GameManager
                if (GameManager.playerHoldTimes.ContainsKey(gameObject))
                {
                    GameManager.playerHoldTimes.Remove(gameObject);
                }
            }
        }

        /// <summary>
        /// Waits for a short delay before making the item interactable again.
        /// </summary>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator WaitForInteractability()
        {
            yield return new WaitForSeconds(0.1f);
            HatRespawn.canBePickedUp = true;
        }

        /// <summary>
        /// Checks if the player is currently holding an item.
        /// </summary>
        /// <returns>True if the player is holding an item, false otherwise.</returns>
        public bool IsHoldingItem()
        {
            return isHoldingItem;
        }
    }
}
