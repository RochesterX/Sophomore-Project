using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Game
{
    /// <summary>
    /// This class controls the main game logic, like starting the game, keeping track of players, 
    /// handling game modes, and deciding when the game ends.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The single instance of this class that can be accessed from anywhere in the game.
        /// </summary>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// The total time (in seconds) for the game to run.
        /// </summary>
        public float time = 180f;

        /// <summary>
        /// A type of event that happens during the game, like when it starts or ends.
        /// </summary>
        public delegate void GameEvent();

        /// <summary>
        /// Event triggered when the game starts.
        /// </summary>
        public event GameEvent StartGameEvent;

        /// <summary>
        /// Event triggered when the game ends.
        /// </summary>
        public event GameEvent EndGameEvent;

        /// <summary>
        /// A list of all the players in the game.
        /// </summary>
        public static List<GameObject> players = new List<GameObject>();

        /// <summary>
        /// A list of colors assigned to each player.
        /// </summary>
        public static List<Color> playerColors = new List<Color>();

        /// <summary>
        /// The distance between players when they spawn.
        /// </summary>
        public float offset = 1f;

        /// <summary>
        /// Whether the background music is turned on.
        /// </summary>
        public static bool music = true;

        /// <summary>
        /// Whether the game is currently over.
        /// </summary>
        public bool gameOver = false;

        /// <summary>
        /// A timer that counts down during the game.
        /// </summary>
        public GameTimer gameTimer;

        /// <summary>
        /// Tracks how long each player has held an item in "keep-away" mode.
        /// </summary>
        public static Dictionary<GameObject, float> playerHoldTimes = new Dictionary<GameObject, float>();

        /// <summary>
        /// The current game mode (e.g., free-for-all, keep-away, or obstacle course).
        /// </summary>
        public static GameMode gameMode = GameMode.freeForAll;

        /// <summary>
        /// The name of the map being played.
        /// </summary>
        public static string map = "Platformer With Headroom";

        /// <summary>
        /// The position where players spawn at the start of the game.
        /// </summary>
        public Vector2 spawnPosition;

        /// <summary>
        /// The position where players spawn in obstacle course mode.
        /// </summary>
        public Vector2 obstacleCourseSpawnPosition;

        /// <summary>
        /// Positions where the hat can spawn in "keep-away" mode.
        /// </summary>
        public List<Vector2> hatSpawnPositions = new List<Vector2>();

        /// <summary>
        /// The canvas that shows the leaderboard during the game.
        /// </summary>
        public Canvas LeaderboardCanvas;

        /// <summary>
        /// The canvas that shows the timer during the game.
        /// </summary>
        public Canvas TimerCanvas;

        /// <summary>
        /// The hat object used in "keep-away" mode.
        /// </summary>
        public GameObject hatObject;

        /// <summary>
        /// The different game modes players can choose from.
        /// </summary>
        public enum GameMode
        {
            /// <summary>
            /// Players compete individually to be the last one standing.
            /// </summary>
            freeForAll,

            /// <summary>
            /// Players compete to hold an item (like a hat) for the longest time.
            /// </summary>
            keepAway,

            /// <summary>
            /// Players race to complete an obstacle course.
            /// </summary>
            obstacleCourse
        }

        /// <summary>
        /// Makes sure there is only one GameManager in the game. If another one exists, it gets deleted.
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
        /// Starts the game and plays background music if it's enabled.
        /// </summary>
        private void Start()
        {
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.StartPlaylist();
            }
            StartGame();
        }

        /// <summary>
        /// Updates the game every frame. In "keep-away" mode, it tracks how long each player holds the item.
        /// </summary>
        private void Update()
        {
            if (gameOver) return;

            if (gameMode == GameMode.keepAway)
            {
                foreach (var player in players)
                {
                    float holdTime = GetPlayerHoldTime(player);
                    UpdatePlayerHoldTime(player, holdTime);
                }
            }
        }

        /// <summary>
        /// Gets how long a player has held the item in "keep-away" mode.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>The time (in seconds) the player has held the item.</returns>
        private float GetPlayerHoldTime(GameObject player)
        {
            UseItem useItem = player.GetComponent<UseItem>();
            if (useItem != null)
            {
                return useItem.holdTime;
            }
            return 0f;
        }

        /// <summary>
        /// Sets up the game based on the selected game mode. This includes spawning players and setting their lives.
        /// </summary>
        public void StartGame()
        {
            GameManager.playerHoldTimes.Clear();
            if (GameManager.players.Count == 0) return;

            StartGameEvent?.Invoke();
            print("Starting game with mode: " + gameMode + " and map: " + map);
            if (gameMode == GameMode.freeForAll)
            {
                foreach (GameObject player in players)
                {
                    player.transform.position = spawnPosition + (offset * (int.Parse(player.name) - 1) * Vector2.right);
                    player.GetComponent<Damageable>().lives = 5;
                }
            }
            if (gameMode == GameMode.keepAway)
            {
                if (gameTimer != null)
                {
                    gameTimer.startTime = time;
                    gameTimer.StartTimer();
                }
                foreach (GameObject player in players)
                {
                    player.transform.position = spawnPosition + (offset * (int.Parse(player.name) - 1) * Vector2.right);
                    player.GetComponent<Damageable>().lives = 0;
                }
            }
            if (gameMode == GameMode.obstacleCourse)
            {
                foreach (GameObject player in players)
                {
                    print("processing player " + player.name);
                    if (obstacleCourseSpawnPosition == Vector2.zero)
                    {
                        player.transform.position = spawnPosition + (offset * (int.Parse(player.name) - 1) * Vector2.right);
                    }
                    else
                    {
                        player.transform.position = obstacleCourseSpawnPosition + (offset * (int.Parse(player.name) - 1) * Vector2.right);
                    }
                    player.GetComponent<Damageable>().lives = 0;
                }
            }
        }

        /// <summary>
        /// Handles what happens when a player dies, like respawning them or ending the game.
        /// </summary>
        /// <param name="player">The player who died.</param>
        public void PlayerDied(Damageable player)
        {
            UseItem useItem = player.GetComponent<UseItem>();
            if (useItem != null && !gameOver)
            {
                useItem.DropItem();
            }

            if (gameMode == GameMode.freeForAll)
            {
                player.lives--;
                if (player.lives <= 0 && !gameOver)
                {
                    player.gameObject.SetActive(false);
                    if (AlivePlayers().Count <= 1)
                    {
                        GameOver();
                    }
                }
                else
                {
                    RespawnPlayer(player.gameObject);
                }
            }
            else
            {
                RespawnPlayer(player.gameObject);
            }
        }

        /// <summary>
        /// Respawns a player at their starting position and resets their damage.
        /// </summary>
        /// <param name="player">The player to respawn.</param>
        private void RespawnPlayer(GameObject player)
        {
            RespawnOnTriggerEnter respawnScript = player.GetComponent<RespawnOnTriggerEnter>();
            if (respawnScript != null)
            {
                //player.transform.position = respawnScript.spawnPoint;
                if (GameManager.gameMode == GameMode.obstacleCourse)
                {
                    player.transform.position = obstacleCourseSpawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                }
                else
                {
                    player.transform.position = spawnPosition + (offset * players.IndexOf(player) * Vector2.right);
                }
                player.GetComponent<Damageable>().ResetDamage();
                player.GetComponent<Damageable>().Respawn();
            }
        }

        /// <summary>
        /// Ends the game and determines the winner based on the current game mode.
        /// </summary>
        /// <remarks>
        /// This method handles the end-of-game logic, such as stopping the timer, hiding UI elements,
        /// and determining the winner based on the <see cref="GameMode"/>. It also triggers the 
        /// <see cref="EndGameEvent"/> for any subscribed listeners.
        /// 
        /// In "free-for-all" mode, the last alive player is declared the winner. In "keep-away" mode, 
        /// the player with the longest hold time wins. In "obstacle course" mode, the winner is determined 
        /// by the <see cref="ObstacleCourse"/> logic.
        /// </remarks>
        /// <example>
        /// <code>
        /// GameManager.Instance.GameOver();
        /// </code>
        /// </example>
        public void GameOver()
        {
            // Mark the game as over
            gameOver = true;

            // Trigger the end game event for any listeners
            EndGameEvent?.Invoke();

            // Hide the leaderboard and timer UI if they exist
            if (LeaderboardCanvas != null)
            {
                LeaderboardCanvas.gameObject.SetActive(false);
            }
            if (TimerCanvas != null)
            {
                TimerCanvas.gameObject.SetActive(false);
            }

            // Handle the winner logic based on the game mode
            if (gameMode == GameMode.freeForAll)
            {
                // In free-for-all mode, the last alive player is the winner
                GameObject winner = FindFirstObjectByType<PlayerMovement>().gameObject;//AlivePlayers()[0];
                print(winner.name + " is the winner");

                // Show the winner's scene and update the win screen
                FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
                print($"Player {int.Parse(winner.name)} won");
                WinScreen.Instance.ShowWinScreen(int.Parse(winner.name));

                // Hide the life display UI
                FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
            }
            else if (gameMode == GameMode.keepAway)
            {
                // In keep-away mode, the player with the longest hold time is the winner
                GameObject winner = null;
                float maxHoldTime = -1f;

                foreach (var player in GameManager.playerHoldTimes)
                {
                    if (player.Value > maxHoldTime)
                    {
                        maxHoldTime = player.Value;
                        winner = player.Key;
                    }
                }

                if (winner != null)
                {
                    print(winner.name + " is the winner with " + maxHoldTime + " seconds!");

                    // Show the winner's scene and update the win screen
                    var cameraMovement = FindFirstObjectByType<PlayerCameraMovement>();
                    if (cameraMovement != null)
                    {
                        cameraMovement.WinScene(winner);
                    }
                    WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);

                    // Hide the life display UI
                    var lifeDisplayManager = FindFirstObjectByType<LifeDisplayManager>();
                    if (lifeDisplayManager != null)
                    {
                        lifeDisplayManager.HideLifeDisplay();
                    }

                    // Move the hat to the winner
                    StartCoroutine(MoveHatToWinner(winner));

                    // Reactivate the hat object
                    if (hatObject != null)
                    {
                        hatObject.SetActive(true);
                        hatObject.GetComponent<Collider2D>().enabled = true;
                        hatObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }
            else if (gameMode == GameMode.obstacleCourse)
            {
                // In obstacle course mode, the winner is determined by the ObstacleCourse logic
                GameObject winner = ObstacleCourse.playerWon;

                print(winner.name + " is the winner!");

                // Show the winner's scene and update the win screen
                FindFirstObjectByType<PlayerCameraMovement>().WinScene(winner);
                WinScreen.Instance.ShowWinScreen(players.IndexOf(winner) + 1);

                // Hide the life display UI
                FindFirstObjectByType<LifeDisplayManager>().HideLifeDisplay();
            }
        }

        /// <summary>
        /// Moves the hat to the winner in "keep-away" mode.
        /// </summary>
        /// <param name="winner">The player who won the game.</param>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        /// <remarks>
        /// This coroutine ensures that the hat is moved to the winner and picked up by them.
        /// It keeps trying until the winner successfully picks up the hat.
        /// </remarks>
        private IEnumerator MoveHatToWinner(GameObject winner)
        {
            while (!winner.GetComponent<UseItem>().IsHoldingItem())
            {
                // Position the hat above the winner
                hatObject.transform.position = winner.transform.position + Vector3.up * 1.5f;

                // Attempt to make the winner pick up the hat
                winner.GetComponent<UseItem>().PickUpItem(hatObject);

                // Wait for the next frame before trying again
                yield return null;
            }
        }

        /// <summary>
        /// Gets a list of all players who are still alive in the game.
        /// </summary>
        /// <returns>A list of players who are still active in the game.</returns>
        /// <remarks>
        /// This method checks all players in the game and returns only those who are still active.
        /// A player is considered "alive" if their GameObject is active in the scene.
        /// </remarks>
        public List<GameObject> AlivePlayers()
        {
            List<GameObject> alivePlayers = new();

            // Check each player to see if they are still active
            foreach (GameObject player in players)
            {
                if (player.activeInHierarchy)
                {
                    alivePlayers.Add(player);
                }
            }

            return alivePlayers;
        }

        /// <summary>
        /// Updates the hold time for a player and refreshes the leaderboard.
        /// </summary>
        /// <param name="player">The player whose hold time is being updated.</param>
        /// <param name="holdTime">The new hold time for the player.</param>
        /// <remarks>
        /// This method updates the player's hold time in the dictionary and refreshes the leaderboard UI.
        /// If the player's hold time is higher than before, the leaderboard is re-sorted.
        /// </remarks>
        public void UpdatePlayerHoldTime(GameObject player, float holdTime)
        {
            bool shouldSort = false;

            // Check if the player already has a recorded hold time
            if (playerHoldTimes.ContainsKey(player))
            {
                // Update the hold time if the new time is greater
                if (holdTime > playerHoldTimes[player])
                {
                    shouldSort = true;
                }
                playerHoldTimes[player] = holdTime;
            }
            else
            {
                // Add the player to the dictionary if they are not already in it
                playerHoldTimes.Add(player, holdTime);
                shouldSort = true;
            }

            // Update the leaderboard UI with the new hold time
            LeaderboardManager.Instance.UpdatePlayerHoldTimeText(player, holdTime);

            // Re-sort the leaderboard if necessary
            if (shouldSort)
            {
                LeaderboardManager.Instance.UpdateLeaderboard();
            }
        }
    }
}