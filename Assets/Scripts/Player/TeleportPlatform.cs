using UnityEngine; using Game; using Music; using Player;

namespace Player
{

public class TeleportPlatform : MonoBehaviour
{
    public Vector2 teleportPoint;
    public string teleportTag;
    public string playerTag = "Player";
    public bool isPlatform = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlatform)
        {
            // Teleports the platform
            if (collision.CompareTag(teleportTag))
            {
                transform.position = teleportPoint;
                if (TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
        else
        {
            // Teleports the player
            if (collision.CompareTag(playerTag))
            {
                collision.transform.position = teleportPoint;
                if (collision.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }
}
}