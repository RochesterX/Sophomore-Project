#if NO
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.SceneManagement;

namespace Music
{
    /// <summary>
    /// Manages the music tracks and layers for the game.
    /// Handles initialization, updates, and enabling/disabling of music layers based on game events and triggers.
    /// </summary>
    public class TrackManager : MonoBehaviour
    {
        /// <summary>
        /// The playlist containing the music tracks and layers.
        /// </summary>
        public Playlist musicTrack;

        /// <summary>
        /// The prefab used to create audio layers.
        /// </summary>
        public GameObject layerPrefab;

        /// <summary>
        /// A list of music layers that persist across scenes.
        /// </summary>
        private List<TrackLayer> persistentLayers = new List<TrackLayer>();

        /// <summary>
        /// The currently active scene.
        /// </summary>
        private Scene currentScene;

        /// <summary>
        /// Ensures the TrackManager is only active if music is enabled in the GameManager.
        /// </summary>
        private void Awake()
        {
            if (!GameManager.music)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initializes the music layers and updates them based on the current scene.
        /// </summary>
        private void Start()
        {
            foreach (var layer in musicTrack.trackLayers)
            {
                if (layer.enableTrigger != TrackLayer.EnableTrigger.Scene)
                {
                    persistentLayers.Add(layer);
                }
            }

            currentScene = GetActiveSceneNotStatistics();
            InitializeLayers();
            UpdateLayers(musicTrack.trackLayers);
        }

        /// <summary>
        /// Checks for scene changes and updates music layers accordingly.
        /// </summary>
        private void Update()
        {
            CheckForRestartability();

            if (currentScene != GetActiveSceneNotStatistics())
            {
                currentScene = GetActiveSceneNotStatistics();
                UpdateLayers(musicTrack.trackLayers);
            }

            if (persistentLayers.Count != 0) UpdateLayers(persistentLayers);
        }

        /// <summary>
        /// Creates and initializes audio sources for each music layer.
        /// </summary>
        private void InitializeLayers()
        {
            foreach (TrackLayer layer in musicTrack.trackLayers)
            {
                AudioSource layerSource = Instantiate(layerPrefab, transform).GetComponent<AudioSource>();
                layerSource.gameObject.name = layer.layerName;
                layerSource.clip = layer.layerTrack;
                layerSource.volume = 0;

                try
                {
                    layerSource.outputAudioMixerGroup = musicTrack.defaultMixer.FindMatchingGroups("Master/" + layer.layerName)[0];
                }
                catch
                {
                    layerSource.outputAudioMixerGroup = musicTrack.defaultMixer.FindMatchingGroups("Master")[0];
                }

                layerSource.Play();
            }
        }

        /// <summary>
        /// Updates the state of the specified music layers based on their triggers and conditions.
        /// </summary>
        /// <param name="layers">The list of music layers to update.</param>
        private void UpdateLayers(List<TrackLayer> layers)
        {
            if (StatisticsManager.PlayerPrefs.GetInt("settingMusic") == 1)
            {
                foreach (TrackLayer layer in layers)
                {
                    DisableLayer(layer);

                    // Handle different enable triggers for the music layers
                    // (e.g., Magnetism, Movement, Toggle, etc.)
                    // Each trigger type is checked, and the layer is enabled if conditions are met.
                    // ...
                }
            }
        }

        /// <summary>
        /// Restarts audio sources if they have stopped playing.
        /// </summary>
        private void CheckForRestartability()
        {
            bool restart = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                AudioSource child = transform.GetChild(i).GetComponent<AudioSource>();
                if (child != null && !child.isPlaying)
                {
                    restart = true;
                    break;
                }
            }

            if (!restart) return;

            for (int i = 0; i < transform.childCount; i++)
            {
                AudioSource child = transform.GetChild(i).GetComponent<AudioSource>();
                if (child != null)
                {
                    child.Stop();
                    child.Play();
                }
            }
        }

        /// <summary>
        /// Enables a specific music layer by setting its animator parameter.
        /// </summary>
        /// <param name="layer">The music layer to enable.</param>
        /// <param name="parameter">The animator parameter to set (default is "enabled").</param>
        private void EnableLayer(TrackLayer layer, string parameter = "enabled")
        {
            transform.Find(layer.layerName).GetComponent<Animator>().SetBool(parameter, true);
        }

        /// <summary>
        /// Disables a specific music layer by resetting all its animator parameters.
        /// </summary>
        /// <param name="layer">The music layer to disable.</param>
        private void DisableLayer(TrackLayer layer)
        {
            foreach (AnimatorControllerParameter parameter in transform.Find(layer.layerName).GetComponent<Animator>().parameters)
            {
                transform.Find(layer.layerName).GetComponent<Animator>().SetBool(parameter.name, false);
            }
        }

        /// <summary>
        /// Gets the currently active scene, excluding the "Statistics Manager Scene".
        /// </summary>
        /// <returns>The active scene.</returns>
        public static Scene GetActiveSceneNotStatistics()
        {
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                if (SceneManager.GetSceneAt(sceneIndex).name != "Statistics Manager Scene")
                {
                    return SceneManager.GetSceneAt(sceneIndex);
                }
            }
            return SceneManager.GetSceneByBuildIndex(0);
        }

        /// <summary>
        /// Stops the music and destroys the TrackManager after a short delay.
        /// </summary>
        public void Stop()
        {
            StartCoroutine(DestroyTrack());
        }

        /// <summary>
        /// Coroutine to destroy the TrackManager after a delay.
        /// </summary>
        /// <returns>An enumerator for the coroutine.</returns>
        public IEnumerator DestroyTrack()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
#endif
