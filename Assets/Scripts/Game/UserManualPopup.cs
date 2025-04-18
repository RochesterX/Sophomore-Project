using UnityEngine;

public class UserManualPopup : MonoBehaviour
{
    public GameObject popupPanel;

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
