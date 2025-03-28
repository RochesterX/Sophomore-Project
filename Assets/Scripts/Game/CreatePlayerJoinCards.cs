using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCardCreator : MonoBehaviour
{
    public static PlayerCardCreator Instance;

    public GameObject playerJoinCardPrefab;

    private void Awake() // Ensures only one instance of PlayerCardCreator exists
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerJoinCard CreateCard() // Creates a player join card
    {
        GameObject card = Instantiate(playerJoinCardPrefab, transform);
        return card.GetComponent<PlayerJoinCard>();
    }
}
