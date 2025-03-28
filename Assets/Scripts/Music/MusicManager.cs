using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public List<Playlist> playlists;
    private Dictionary<string, Playlist> sceneToPlaylist = new Dictionary<string, Playlist>();
    public GameObject songPrefab;

    private void Awake() // Creates only one MusicManager instance at a time
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

    public void StartPlaylist() // Starts music playlist for each scene
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

    public void StartPlaylist(string scene) // Sets music for Title Screen
    {
        if (GetActiveSceneNotTitleScreen() == "Player Select") return;
        StopAllCoroutines();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(PlayPlaylist(sceneToPlaylist[scene]));
    }

    private IEnumerator PlayPlaylist(Playlist playlist)
    {
        while (true)
        {
            // Shuffles the playlist
            List<AudioClip> randomized = new List<AudioClip>(playlist.songs);
            for (int i = 0; i < randomized.Count; i++)
            {
                AudioClip temp = randomized[i];
                int randomIndex = Random.Range(i, randomized.Count);
                randomized[i] = randomized[randomIndex];
                randomized[randomIndex] = temp;
            }

            // Starts the music in the playlist
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

    public static string GetActiveSceneNotTitleScreen() // Finds the scene name besides Title Screen
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
