using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        UnloadGameScene();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadGameScene()
    {
        try
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
        catch
        {
            Debug.Log("No game scene to unload");
        }
    }

    private void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().escapeKey.wasPressedThisFrame)
        {
            UnloadGameScene();
        }
    }
}
