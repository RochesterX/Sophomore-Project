using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<PlayerJoinCard> cards;
    [SerializeField] private InputActionAsset playerActions;

    public List<Color> playerColors;

    public GameObject playerSelect;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        print("Player joined");

        //playerInput.transform.SetParent(transform);
        DontDestroyOnLoad(playerInput.gameObject);

        PlayerJoinCard card = PlayerCardCreator.Instance.CreateCard();
        card.playerNumber = GameManager.players.Count + 1;
        cards.Add(card);

        GameManager.players.Add(playerInput.gameObject);

        Colorize(GameManager.players.Count - 1);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Destroy(playerInput.gameObject);
        GameManager.players.Remove(playerInput.gameObject);
        print("Player left");
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            print("A PlayerManager already exists.");
            Destroy(this.gameObject);
        }
    }

    public void StartGame()
    {
        HubManager.Instance.LoadScene(GameManager.map);
    }

    private void Colorize(int index)
    {
        GameObject player = GameManager.players[index];

        Color color = playerColors[(GameManager.players.Count - 1) % playerColors.Count];
        float tint = Mathf.Floor((GameManager.players.Count - 1) / playerColors.Count);
        color = (color + color + Color.white * tint) / (tint + 2);

        ApplyColor(player, color);
        ApplyColor(cards[GameManager.players.IndexOf(player)].playerPreview, color);
    }

    private void ApplyColor(GameObject obj, Color color)
    {        
        if (obj.TryGetComponent<SpriteRenderer>(out _))
        {
            obj.GetComponent<SpriteRenderer>().color = color;
        }
        foreach (Transform child in obj.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out _))
            {
                ApplyColor(child.gameObject, color);
            }
        }
    }
}
