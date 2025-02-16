using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> players;
    public List<PlayerJoinCard> cards;
    [SerializeField] private InputActionAsset playerActions;

    public List<Color> playerColors;

    public GameObject playerSelect;

    private Vector2 spawnPosition;

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
        playerInput.transform.SetParent(transform);
        
        PlayerJoinCard card = PlayerCardCreator.Instance.CreateCard(playerInput);
        card.playerNumber = players.Count + 1;
        cards.Add(card);

        playerInput.transform.position = spawnPosition;
        players.Add(playerInput.gameObject);
        Colorize(players.Count - 1);
        print("Player joined");
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Destroy(playerInput.gameObject);
        players.Remove(playerInput.gameObject);
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

        spawnPosition = transform.position;
    }

    public void StartGame()
    {
        foreach (Camera camera in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            camera.enabled = !camera.enabled;
        }

        foreach (GameObject player in players)
        {
            player.transform.position = spawnPosition;
            player.GetComponent<Damageable>().damage = 0f;
        }

        Destroy(playerSelect);
    }

    private void Colorize(int index)
    {
        GameObject player = players[index];

        Color color = playerColors[(players.Count - 1) % playerColors.Count];
        float tint = Mathf.Floor((players.Count - 1) / playerColors.Count);
        color = (color + color + Color.white * tint) / (tint + 2);

        ApplyColor(player, color);
        ApplyColor(cards[players.IndexOf(player)].playerPreview, color);
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
