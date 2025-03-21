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
            //GetComponent<Damageable>().Respawn();
            if (TryGetComponent(out Damageable damageable))
            {
                damageable.Damage(9999f);
            }
        }
    }
}