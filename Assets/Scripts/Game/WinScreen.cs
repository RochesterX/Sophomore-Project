using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game;
using Music;
using Player;

namespace Game
{
    /// <summary>
    /// Manages the win screen display for the game.
    /// Displays the winning player's information and triggers the win screen animation.
    /// </summary>
    public class WinScreen : MonoBehaviour
    {
        /// <summary>
        /// A singleton instance of the <see cref="WinScreen"/> class.
        /// Ensures only one instance of the WinScreen exists at a time.
        /// </summary>
        public static WinScreen Instance;

        /// <summary>
        /// A list of text elements used to display player information on the win screen.
        /// </summary>
        public List<TextMeshProUGUI> playerTexts;

        /// <summary>
        /// Ensures only one instance of the WinScreen exists.
        /// Destroys duplicate instances if they are created.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Displays the win screen for the specified player.
        /// Updates the text and color of the win screen to reflect the winning player.
        /// </summary>
        /// <param name="player">The number of the winning player (1-based index).</param>
        public void ShowWinScreen(int player)
        {
            // Validate the player index
            if (player - 1 < 0 || player - 1 >= GameManager.playerColors.Count)
            {
                return;
            }

            // Update the text and color for each player text element
            foreach (TextMeshProUGUI playerText in playerTexts)
            {
                playerText.text = "Player " + player;
                if (playerText.color != Color.black)
                {
                    playerText.color = GameManager.playerColors[player - 1];
                }
            }

            // Trigger the win screen animation
            Animator animator = GetComponent<Animator>();
            if (animator == null)
            {
                return;
            }

            animator.SetTrigger("win");
        }
    }
}