using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> players;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private InputActionAsset playerActions;


    [Header("Debug")]
    [SerializeField] private InputActionAsset player2Actions;

    private PlayerCameraMovement playerCamera;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;

        playerCamera = FindFirstObjectByType<PlayerCameraMovement>();
    }

    private void Update()
    {
        if (InputSystem.actions.FindAction("Join Player").WasPressedThisFrame())
        {
            JoinPlayer(playerActions);
        }

        if (Keyboard.current.periodKey.wasPressedThisFrame)
        {
            JoinPlayer(player2Actions);
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPosition;
        players.Add(playerInput.gameObject);
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
        }
    }

    private void JoinPlayer(InputActionAsset actions)
    {
        PlayerInput player = GetComponent<PlayerInputManager>().JoinPlayer(-1, -1, null, Keyboard.current);
        print(player == null);
        player.actions = actions;
    }
}
