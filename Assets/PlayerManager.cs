using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPosition;

    private PlatformerCameraMovement playerCamera;

    private void Start()
    {
        GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        GetComponent<PlayerInputManager>().onPlayerLeft += OnPlayerLeft;

        playerCamera = FindFirstObjectByType<PlatformerCameraMovement>();
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPosition;
        playerCamera.players.Add(playerInput.gameObject);
        print("Player joined");
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Destroy(playerInput.gameObject);
        playerCamera.players.Remove(playerInput.gameObject);
        print("Player left");
    }
}
