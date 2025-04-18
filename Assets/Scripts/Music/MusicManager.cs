using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.SceneManagement;

namespace Music
{
    /// <summary>
    /// Manages the music playlists for the game.
    /// Handles the initialization, playback, and shuffling of music tracks based on the active scene.
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
        /// <summary>
        /// A singleton instance of the <see cref="MusicManager"/> class.
        /// Ensures only one instance of the MusicManager exists at a time.
        /// </summary>
        public static MusicManager Instance;

        /// <summary>
        /// A list of playlists available in the game.
        /// </summary>
        public List<Playlist> playlists;

        /// <summary>
        /// A dictionary mapping scene names to their corresponding playlists.
        /// </summary>
        private Dictionary<string, Playlist> sceneToPlaylist = new Dictionary<string, Playlist>();

        /// <summary>
        /// The prefab used to create audio sources for playing songs.
        /// </summary>
        public GameObject songPrefab;

        /// <summary>
        /// Ensures only one instance of the MusicManager exists and initializes the scene-to-playlist mapping.
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

            foreach (Playlist playlist in playlists)
            {
                foreach (string scene in playlist.trackScenes)
                {
                    sceneToPlaylist.Add(scene, playlist);
                }
            }
        }

        /// <summary>
        /// Starts the music playlist for the current active scene.
        /// </summary>
        public void StartPlaylist()
        {
            if (GetActiveSceneNotTitleScreen() == "Player Select") return;

            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            try
            {
                StartCoroutine(PlayPlaylist(sceneToPlaylist[GetActiveSceneNotTitleScreen()]));
            }
            catch (System.Exception)
            {
                print("No playlist found for this scene: " + GetActiveSceneNotTitleScreen());
            }
        }

        /// <summary>
        /// Starts the music playlist for a specific scene.
        /// </summary>
        /// <param name="scene">The name of the scene for which to start the playlist.</param>
        public void StartPlaylist(string scene)
        {
            if (GetActiveSceneNotTitleScreen() == "Player Select") return;

            StopAllCoroutines();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(PlayPlaylist(sceneToPlaylist[scene]));
        }

        /// <summary>
        /// Plays the specified playlist by shuffling and playing its songs in sequence.
        /// </summary>
        /// <param name="playlist">The playlist to play.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        private IEnumerator PlayPlaylist(Playlist playlist)
        {
            while (true)
            {
                // Shuffle the playlist
                List<AudioClip> randomized = new List<AudioClip>(playlist.songs);
                for (int i = 0; i < randomized.Count; i++)
                {
                    AudioClip temp = randomized[i];
                    int randomIndex = Random.Range(i, randomized.Count);
                    randomized[i] = randomized[randomIndex];
                    randomized[randomIndex] = temp;
                }

                // Play each song in the shuffled playlist
                foreach (AudioClip song in randomized)
                {
                    AudioSource songInstance = Instantiate(songPrefab, transform).GetComponent<AudioSource>();
                    songInstance.clip = song;
                    songInstance.volume = playlist.volume;
                    songInstance.Play();

                    if (playlist.shuffleTime > 0f)
                    {
                        yield return new WaitForSeconds(playlist.shuffleTime);
                        float time = 0f;
                        while (time < 5f)
                        {
                            songInstance.volume = playlist.volume * (1 - time / 5f);
                            time += Time.deltaTime;
                            yield return null;
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(song.length);
                    }

                    Destroy(songInstance.gameObject);
                }
            }
        }

        /// <summary>
        /// Gets the name of the currently active scene, excluding the "Title Screen".
        /// </summary>
        /// <returns>The name of the active scene, or "Title Screen" if no other scene is active.</returns>
        public static string GetActiveSceneNotTitleScreen()
        {
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                if (SceneManager.GetSceneAt(sceneIndex).name != "Title Screen")
                {
                    return SceneManager.GetSceneAt(sceneIndex).name;
                }
            }
            return "Title Screen";
        }
    }
}