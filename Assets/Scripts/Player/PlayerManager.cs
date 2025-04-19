using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class manages player-related functionality, such as joining, leaving, and assigning colors.
    /// It also handles starting the game once players have joined.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the <see cref="PlayerManager"/> class.
        /// </summary>
        public static PlayerManager Instance;

        /// <summary>
        /// A list of player join cards, which represent players in the UI.
        /// </summary>
        public List<PlayerJoinCard> cards;

        /// <summary>
        /// The input actions used by players.
        /// </summary>
        [SerializeField] private InputActionAsset playerActions;

        /// <summary>
        /// A list of colors assigned to players for identification.
        /// </summary>
        public List<Color> playerColors;

        /// <summary>
        /// The UI element used for player selection.
        /// </summary>
        public GameObject playerSelect;

        /// <summary>
        /// Indicates whether the game has started.
        /// </summary>
        private bool gameStarted = false;

        /// <summary>
        /// Initializes the singleton instance of the <see cref="PlayerManager"/>.
        /// </summary>
        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// Subscribes to player join and leave events.
        /// </summary>
        private void Start()
        {
            GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
            GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;
        }

        /// <summary>
        /// Handles logic for when a player joins the game.
        /// </summary>
        /// <param name="playerInput">The <see cref="PlayerInput"/> of the player who joined.</param>
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            // Prevent players from joining after the game has started
            if (gameStarted)
            {
                Destroy(playerInput.gameObject);
                return;
            }

            print("Player joined");

            // Ensure the player object persists across scenes
            DontDestroyOnLoad(playerInput.gameObject);

            // Create a player join card for the new player
            if (PlayerCardCreator.Instance == null)
            {
                return;
            }
            PlayerJoinCard card = PlayerCardCreator.Instance.CreateCard();
            if (card == null)
            {
                return;
            }

            // Assign a player number and add the card to the list
            card.playerNumber = GameManager.players.Count + 1;
            cards.Add(card);

            // Initialize the player list in the GameManager if it doesn't exist
            if (GameManager.players == null)
            {
                GameManager.players = new List<GameObject>();
            }

            playerInput.gameObject.name = card.playerNumber.ToString();
            // Add the player to the GameManager's player list and assign a color
            GameManager.players.Add(playerInput.gameObject);
            Colorize(GameManager.players.Count - 1);
        }

        /// <summary>
        /// Handles logic for when a player leaves the game.
        /// </summary>
        /// <param name="playerInput">The <see cref="PlayerInput"/> of the player who left.</param>
        private void OnPlayerLeft(PlayerInput playerInput)
        {
            // Remove the player from the game and destroy their object
            Destroy(playerInput.gameObject);
            GameManager.players.Remove(playerInput.gameObject);
            print("Player left");
        }

        /// <summary>
        /// Initializes the singleton instance of the <see cref="PlayerManager"/>.
        /// </summary>
        private void Init()
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
        /// Starts the game if at least one player has joined.
        /// </summary>
        public void StartGame()
        {
            // Prevent starting the game if no players have joined
            if (GameManager.players.Count == 0)
            {
                return;
            }

            gameStarted = true;

            // Load the selected map
            HubManager.Instance.LoadScene(GameManager.map);
        }

        /// <summary>
        /// Assigns a unique color to a player and their associated UI elements.
        /// </summary>
        /// <param name="index">The index of the player in the player list.</param>
        private void Colorize(int index)
        {
            // Get the player object and assign a base color
            GameObject player = GameManager.players[index];
            Color color = playerColors[(GameManager.players.Count - 1) % playerColors.Count];

            // Adjust the color tint based on the number of players
            float tint = Mathf.Floor((GameManager.players.Count - 1) / playerColors.Count);
            color = (color + color + Color.white * tint) / (tint + 2);

            // Add the color to the GameManager's player color list
            GameManager.playerColors.Add(color);

            // Apply the color to the player and their UI preview
            ApplyColor(player, color);
            ApplyColor(cards[GameManager.players.IndexOf(player)].playerPreview, color);
        }

        /// <summary>
        /// Applies a color to a GameObject and its children.
        /// </summary>
        /// <param name="obj">The GameObject to colorize.</param>
        /// <param name="color">The color to apply.</param>
        private void ApplyColor(GameObject obj, Color color)
        {
            // Apply the color to the object's SpriteRenderer, if it has one
            if (obj.TryGetComponent<SpriteRenderer>(out _))
            {
                obj.GetComponent<SpriteRenderer>().color = color;
            }

            // Recursively apply the color to all child objects
            foreach (Transform child in obj.transform)
            {
                if (child.TryGetComponent<SpriteRenderer>(out _))
                {
                    ApplyColor(child.gameObject, color);
                }
            }
        }
    }
}

