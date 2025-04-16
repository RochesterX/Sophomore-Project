using UnityEngine; using Game; using Music; using Player;
namespace Game
{
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

    private void UpdateVisibility() // Sets object visible if playing keep away mode
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
}