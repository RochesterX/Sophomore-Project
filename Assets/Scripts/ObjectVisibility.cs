using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    void Start()
    {
        UpdateVisibility();
    }

    void Update()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (GameManager.gameMode == GameManager.GameMode.keepAway)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
