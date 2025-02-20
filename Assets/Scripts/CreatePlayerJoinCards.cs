using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCardCreator : MonoBehaviour
{
    public static PlayerCardCreator Instance;

    public GameObject playerJoinCardPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerJoinCard CreateCard()
    {
        GameObject card = Instantiate(playerJoinCardPrefab, transform);
        return card.GetComponent<PlayerJoinCard>();
    }
}
