using UnityEngine;

public class RespawnOnTriggerEnter : MonoBehaviour
{
    public Vector2 spawnPoint;
    public bool spawnPointIsInitialPosition = false;
    public string respawnTag;

    private void Start()
    {
        if (spawnPointIsInitialPosition)
        {
            spawnPoint = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(respawnTag))
        {
            transform.position = spawnPoint;
            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
