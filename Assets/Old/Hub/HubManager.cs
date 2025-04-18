using System.Linq;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Game{
public class HubManager : MonoBehaviour
{
    public static HubManager Instance;
    public GameObject hubCamera;
    public GameObject gameButtonsParent;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (hubCamera.GetComponent<AudioListener>() == null)
        {
            hubCamera.AddComponent<AudioListener>();
        }
        hubCamera.SetActive(true);
        MusicManager.Instance.StartPlaylist();
        print("Game started");
        }

    public void LoadScene(string sceneName)
    {
        UnloadGameScene();
        hubCamera.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        var activeCamera = Camera.main;
        if (activeCamera != null && activeCamera.GetComponent<AudioListener>() == null)
        {
            activeCamera.gameObject.AddComponent<AudioListener>();
        }
        MusicManager.Instance.StartPlaylist();
        print("Loading scene: " + sceneName);
    }

    public void UnloadGameScene()
    {
        hubCamera.SetActive(true);
        try
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
        catch {}
        ChangeGameButtonsInteractability(false);
    }

   private void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame)
        {
            UnloadGameScene();
            ChangeGameButtonsInteractability(true);
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
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.StartPlaylist("Title Screen");
            }
            var cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            if (cameras != null)
            {
                foreach (Camera camera in cameras)
                {
                    camera.enabled = false;
                }
            }
            GameManager.players?.Clear();
            GameManager.playerColors?.Clear();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.gameOver = false;
            }
            SceneManager.LoadScene("Title Screen");

        }
    }

    private void ChangeGameButtonsInteractability(bool interactable)
    {
        gameButtonsParent.transform.parent.gameObject.SetActive(interactable);
        foreach (Transform button in gameButtonsParent.transform)
        {
            button.GetComponent<Button>().interactable = interactable;
        }
    }
}
}