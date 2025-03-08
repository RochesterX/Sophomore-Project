using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemizer : MonoBehaviour
{
    private void Update()
    {
        foreach (EventSystem system in FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
        {
            if (system == GetComponent<EventSystem>()) continue;
            Destroy(system.gameObject);
        }
    }
}
