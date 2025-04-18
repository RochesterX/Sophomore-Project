using System.Linq;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Game
{
    /// <summary>
    /// This class manages the hub area of the game, including loading and unloading game scenes,
    /// controlling the hub camera, and managing game buttons.
    /// </summary>
    public class HubManager : MonoBehaviour
    {
        /// <summary>
        /// A single instance of this class that can be accessed from anywhere.
        /// </summary>
        public static HubManager Instance;

        /// <summary>
        /// The camera used in the hub area.
        /// </summary>
        public GameObject hubCamera;

        /// <summary>
        /// The parent object containing all game buttons in the hub.
        /// </summary>
        public GameObject gameButtonsParent;

        /// <summary>
        /// Sets up the hub manager and ensures only one instance exists.
        /// </summary>
        private void Start()
        {
            // Ensure there is only one HubManager in the game
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            // Add an AudioListener to the hub camera if it doesn't already have one
            if (hubCamera.GetComponent<AudioListener>() == null)
            {
                hubCamera.AddComponent<AudioListener>();
            }

            // Activate the hub camera and start the music playlist
            hubCamera.SetActive(true);
            MusicManager.Instance.StartPlaylist();
            print("Game started");
        }

        /// <summary>
        /// Loads a new game scene and disables the hub camera.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            // Unload the current game scene and disable the hub camera
            UnloadGameScene();
            hubCamera.SetActive(false);

            // Load the new scene
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            // Ensure the active camera has an AudioListener
            var activeCamera = Camera.main;
            if (activeCamera != null && activeCamera.GetComponent<AudioListener>() == null)
            {
                activeCamera.gameObject.AddComponent<AudioListener>();
            }

            // Start the music playlist for the new scene
            MusicManager.Instance.StartPlaylist();
            print("Loading scene: " + sceneName);
        }

        /// <summary>
        /// Unloads the current game scene and reactivates the hub camera.
        /// </summary>
        public void UnloadGameScene()
        {
            // Reactivate the hub camera
            hubCamera.SetActive(true);

            try
            {
                // Unload the currently loaded game scene
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            }
            catch
            {
                // Ignore errors if no additional scene is loaded
            }

            // Disable interaction with game buttons
            ChangeGameButtonsInteractability(false);
        }

        /// <summary>
        /// Handles input and resets the game when the escape key is pressed.
        /// </summary>
        private void Update()
        {
            if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame)
            {
                // Unload the current game scene and enable game buttons
                UnloadGameScene();
                ChangeGameButtonsInteractability(true);

                // Remove all players and reset the game state
                if (GameManager.players != null)
                {
                    foreach (GameObject player in GameManager.players.ToList())
                    {
                        GameManager.players.Remove(player);
                        if (player != null)
                        {
                            Destroy(player);
                        }
                    }
                }

                // Restart the music playlist for the title screen
                if (MusicManager.Instance != null)
                {
                    MusicManager.Instance.StartPlaylist("Title Screen");
                }

                // Disable all cameras in the scene
                var cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
                if (cameras != null)
                {
                    foreach (Camera camera in cameras)
                    {
                        camera.enabled = false;
                    }
                }

                // Clear player data and reset the game state
                GameManager.players?.Clear();
                GameManager.playerColors?.Clear();
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.gameOver = false;
                }

                // Load the title screen
                SceneManager.LoadScene("Title Screen");
            }
        }

        /// <summary>
        /// Enables or disables interaction with the game buttons in the hub.
        /// </summary>
        /// <param name="interactable">True to enable interaction, false to disable it.</param>
        private void ChangeGameButtonsInteractability(bool interactable)
        {
            // Show or hide the game buttons
            gameButtonsParent.transform.parent.gameObject.SetActive(interactable);

            // Enable or disable each button
            foreach (Transform button in gameButtonsParent.transform)
            {
                button.GetComponent<Button>().interactable = interactable;
            }
        }
    }
}