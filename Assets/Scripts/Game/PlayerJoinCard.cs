using TMPro;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// This class represents a player join card, displaying the player's number
    /// and preview in the game lobby.
    /// </summary>
    public class PlayerJoinCard : MonoBehaviour
    {
        /// <summary>
        /// The preview object representing the player.
        /// </summary>
        public GameObject playerPreview;

        /// <summary>
        /// The number assigned to the player.
        /// </summary>
        public int playerNumber;

        /// <summary>
        /// The text element displaying the player's number.
        /// </summary>
        public TextMeshProUGUI playerNumberText;

        /// <summary>
        /// Sets the player's number text when the game starts.
        /// </summary>
        private void Start()
        {
            // Display the player's number on the join card
            playerNumberText.text = playerNumber.ToString();
        }
    }
}