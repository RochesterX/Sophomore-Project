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
            Debug.Log("A HubManager already exists.");
            Destroy(this.gameObject);
        }
        hubCamera.SetActive(true);
        MusicManager.Instance.StartPlaylist();
    }

    public void LoadScene(string sceneName)
    {
        UnloadGameScene();
        hubCamera.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        MusicManager.Instance.StartPlaylist();
        print("Loading scene: playing solmg" + sceneName);
    }

    public void UnloadGameScene()
    {
        hubCamera.SetActive(true);
        try
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
        catch
        {
            Debug.Log("No game scene to unload");
        }

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