using UnityEngine;

public class RespawnOnTriggerEnter : MonoBehaviour
{
    public Vector2 spawnPoint;
    public bool spawnPointIsInitialPosition = false;
    public string respawnTag;

    private void Start() // Set the spawn point to the initial maps spawn point
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
            if (TryGetComponent(out Damageable damageable))
            {
                print("Voided out " + other.name);
                damageable.Damage(9999f);
            }
        }
    }
}