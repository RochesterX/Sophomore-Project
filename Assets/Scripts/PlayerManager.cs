using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> players;
    [SerializeField] private InputActionAsset playerActions;

    public List<Color> playerColors;

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
        playerInput.transform.position = spawnPosition;
        players.Add(playerInput.gameObject);
        Colorize(playerInput.gameObject);
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

    private void Colorize(GameObject player)
    {
        Color color = playerColors[(players.Count - 1) % playerColors.Count];
        float tint = Mathf.Floor((players.Count - 1) / playerColors.Count);
        color = (color + color + Color.white * tint) / (tint + 2);
        
        if (player.TryGetComponent<SpriteRenderer>(out _))
        {
            player.GetComponent<SpriteRenderer>().color = color;
        }
        foreach (Transform child in player.transform)
        {
            if (child.TryGetComponent<SpriteRenderer>(out _))
            {
                Colorize(child.gameObject);
            }
        }
    }
}
