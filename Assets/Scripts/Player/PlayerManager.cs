using System.Collections;
using System.Collections.Generic;
using UnityEngine; using Game; using Music; using Player;
using UnityEngine.InputSystem;
namespace Player
{

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<PlayerJoinCard> cards;
    [SerializeField] private InputActionAsset playerActions;
    public List<Color> playerColors;
    public GameObject playerSelect;
    private bool gameStarted = false;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput playerInput) // Adds a player when they join
    {
        if (gameStarted)
        {
            Destroy(playerInput.gameObject);
            return;
        }
        print("Player joined");
        DontDestroyOnLoad(playerInput.gameObject);
        if (PlayerCardCreator.Instance == null)
        {
            return;
        }
        PlayerJoinCard card = PlayerCardCreator.Instance.CreateCard();
        if (card == null)
        {
            return;
        }
        card.playerNumber = GameManager.players.Count + 1;
        cards.Add(card);
        if (GameManager.players == null)
        {
            GameManager.players = new List<GameObject>();
        }
        GameManager.players.Add(playerInput.gameObject);
        Colorize(GameManager.players.Count - 1);
    }


    private void OnPlayerLeft(PlayerInput playerInput) // Removes the player if they leave
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
            Destroy(this.gameObject);
        }
    }

    public void StartGame() // Allows game to start after a player has joined
    {
        if (GameManager.players.Count == 0)
        {
            return;
        }
        gameStarted = true;
        HubManager.Instance.LoadScene(GameManager.map);
    }

    private void Colorize(int index) // Pairs each player with a unique color
    {
        GameObject player = GameManager.players[index];
        Color color = playerColors[(GameManager.players.Count - 1) % playerColors.Count];
        float tint = Mathf.Floor((GameManager.players.Count - 1) / playerColors.Count);
        color = (color + color + Color.white * tint) / (tint + 2);
        GameManager.playerColors.Add(color);
        ApplyColor(player, color);
        ApplyColor(cards[GameManager.players.IndexOf(player)].playerPreview, color);
    }

    private void ApplyColor(GameObject obj, Color color) // Applies a color to each player
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
}}