using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> players;
    [SerializeField] private Vector2 spawnPosition;

    private PlatformerCameraMovement playerCamera;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;

        playerCamera = FindFirstObjectByType<PlatformerCameraMovement>();
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
}
