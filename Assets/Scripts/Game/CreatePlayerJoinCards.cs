using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>
    /// This class is used to create cards for players when they join the game.
    /// </summary>
    public class PlayerCardCreator : MonoBehaviour
    {
        /// <summary>
        /// A single instance of this class that can be accessed from anywhere.
        /// </summary>
        public static PlayerCardCreator Instance;

        /// <summary>
        /// The template used to create new player cards.
        /// </summary>
        public GameObject playerJoinCardPrefab;

        /// <summary>
        /// Makes sure there is only one PlayerCardCreator in the game.
        /// </summary>
        private void Awake()
        {
            // If this is the first instance, set it as the main one.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                // If another instance already exists, remove this one.
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Creates a new player card and returns it.
        /// </summary>
        /// <returns>The new player card, or nothing if it couldn't be created.</returns>
        public PlayerJoinCard CreateCard()
        {
            try
            {
                // Make a new player card using the template.
                GameObject card = Instantiate(playerJoinCardPrefab, transform);

                // Return the player card so it can be used.
                return card.GetComponent<PlayerJoinCard>();
            }
            catch
            {
                // If something goes wrong, return nothing.
                return null;
            }
        }
    }
}


