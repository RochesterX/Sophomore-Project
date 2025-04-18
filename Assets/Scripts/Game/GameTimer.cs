using TMPro;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// This class manages the game's countdown timer.
    /// It starts, updates, and stops the timer, and ends the game when time runs out.
    /// </summary>
    public class GameTimer : MonoBehaviour
    {
        /// <summary>
        /// The starting time for the timer, in seconds.
        /// </summary>
        public float startTime = 180f;

        /// <summary>
        /// The time remaining on the timer, in seconds.
        /// </summary>
        private float timeRemaining;

        /// <summary>
        /// Indicates whether the timer is currently running.
        /// </summary>
        private bool timerRunning = false;

        /// <summary>
        /// The UI text element that displays the timer.
        /// </summary>
        public Text timerText;

        /// <summary>
        /// The TextMeshPro element that displays the timer.
        /// </summary>
        [SerializeField] private TextMeshProUGUI timer;

        /// <summary>
        /// Sets up the timer when the game starts.
        /// </summary>
        private void Start()
        {
            // Set the timer to the starting time and display the initial value
            timeRemaining = startTime;
            timer.text = "3:00.00";
            UpdateTimerDisplay();
        }

        /// <summary>
        /// Updates the timer every frame to show the time remaining.
        /// </summary>
        private void Update()
        {
            if (timerRunning)
            {
                // Decrease the time remaining
                timeRemaining -= Time.deltaTime;

                // Stop the timer if time runs out
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    timerRunning = false;
                    OnTimerEnd();
                }

                // Update the timer display
                UpdateTimerDisplay();
            }
        }

        /// <summary>
        /// Starts the timer if it is not already running.
        /// </summary>
        public void StartTimer()
        {
            if (!timerRunning)
            {
                // Reset the timer and start it
                timeRemaining = startTime;
                timerRunning = true;
            }
        }

        /// <summary>
        /// Updates the timer display to show the current time remaining.
        /// </summary>
        private void UpdateTimerDisplay()
        {
            // Calculate minutes and seconds from the remaining time
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            // Format the time as "MM:SS" and update the UI
            timer.text = string.Format("{0}:{1:D2}", minutes, seconds);
        }

        /// <summary>
        /// Ends the game when the timer reaches zero.
        /// </summary>
        private void OnTimerEnd()
        {
            // Notify the GameManager that the game is over
            GameManager.Instance.GameOver();
        }
    }
}


