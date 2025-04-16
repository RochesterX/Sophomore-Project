using UnityEngine; using Game; using Music; using Player;
using UnityEngine.EventSystems;

namespace Game
{
public class EventSystemizer : MonoBehaviour
{
    private void Update() // Ensures only one instance of EventSystem exists
    {
        foreach (EventSystem system in FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
        {
            if (system == GetComponent<EventSystem>()) continue;
            Destroy(system.gameObject);
        }
    }
}
}