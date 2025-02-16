using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    public GameObject hubCamera;
    public GameObject gameButtonsParent;

    private void Start()
    {
        hubCamera.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        UnloadGameScene();
        hubCamera.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
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
        }
    }

    private void ChangeGameButtonsInteractability(bool interactable)
    {
        foreach (Transform button in gameButtonsParent.transform)
        {
            button.GetComponent<Button>().interactable = interactable;
        }
    }
}
