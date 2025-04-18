using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// This class manages the leaderboard, including initializing player icons,
    /// updating player positions, and displaying hold times.
    /// </summary>
    public class LeaderboardManager : MonoBehaviour
    {
        /// <summary>
        /// A single instance of this class that can be accessed from anywhere.
        /// </summary>
        public static LeaderboardManager Instance { get; private set; }

        /// <summary>
        /// The parent object that contains all player icons on the leaderboard.
        /// </summary>
        [SerializeField] private GameObject playersParent;

        /// <summary>
        /// The prefab used to represent a player on the leaderboard.
        /// </summary>
        [SerializeField] private GameObject playerPrefab;

        /// <summary>
        /// The prefab used for the leaderboard icon of each player.
        /// </summary>
        [SerializeField] private GameObject leaderboardIconPrefab;

        /// <summary>
        /// A dictionary mapping each player to their corresponding leaderboard icon.
        /// </summary>
        private Dictionary<GameObject, GameObject> playerIcons = new Dictionary<GameObject, GameObject>();

        /// <summary>
        /// Ensures only one instance of the LeaderboardManager exists.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initializes the leaderboard when the game starts.
        /// </summary>
        private void Start()
        {
            InitializeLeaderboard();
        }

        /// <summary>
        /// Creates the leaderboard icons for each player and positions them.
        /// </summary>
        private void InitializeLeaderboard()
        {
            // Adjust the position of the parent container for player icons
            RectTransform parentRectTransform = playersParent.GetComponent<RectTransform>();
            parentRectTransform.anchoredPosition = new Vector2(-10f, 10f);

            // Create a leaderboard icon for each player
            foreach (GameObject player in GameManager.players)
            {
                Transform parent = Instantiate(playerPrefab, playersParent.transform).transform;
                GameObject leaderboardIcon = Instantiate(leaderboardIconPrefab, parent);

                // Set the color of the leaderboard icon based on the player's color
                leaderboardIcon.GetComponentInChildren<Image>().color = GameManager.playerColors[GameManager.players.IndexOf(player)];
                playerIcons[player] = parent.gameObject;
            }
        }

        /// <summary>
        /// Updates the leaderboard by sorting players based on their hold times
        /// and adjusting their positions.
        /// </summary>
        public void UpdateLeaderboard()
        {
            // Sort players by their hold times in descending order
            List<KeyValuePair<GameObject, float>> sortedList = new List<KeyValuePair<GameObject, float>>(GameManager.playerHoldTimes);
            sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            // Update the position and rank of each player on the leaderboard
            for (int i = 0; i < sortedList.Count; i++)
            {
                var player = sortedList[i];
                playerIcons[player.Key].transform.SetSiblingIndex(i);

                // Update the rank text for the player
                TextMeshProUGUI[] textComponents = playerIcons[player.Key].GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textComponent in textComponents)
                {
                    if (textComponent.name == "Position Text")
                    {
                        textComponent.text = "#" + (i + 1).ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the hold time text for a specific player on the leaderboard.
        /// </summary>
        /// <param name="player">The player whose hold time is being updated.</param>
        /// <param name="holdTime">The new hold time to display.</param>
        public void UpdatePlayerHoldTimeText(GameObject player, float holdTime)
        {
            if (playerIcons.ContainsKey(player))
            {
                // Find and update the hold time text for the player
                TextMeshProUGUI[] textComponents = playerIcons[player].GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textComponent in textComponents)
                {
                    if (textComponent.name == "Text (TMP)")
                    {
                        int minutes = Mathf.FloorToInt(holdTime / 60F);
                        int seconds = Mathf.FloorToInt(holdTime % 60F);
                        textComponent.text = string.Format("{0:0}:{1:00}", minutes, seconds);
                        break;
                    }
                }
            }
        }
    }
}