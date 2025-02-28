using UnityEngine;

public class TeleportPlatform : MonoBehaviour
{
    public Vector2 teleportPoint;
    //public bool teleportPosition = false;
    public string teleportTag;

    //private void Start()
    //{
        //if (teleportPosition)
        //{
            //teleportPoint = transform.position;
        //}
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(teleportTag))
        {
            transform.position = teleportPoint;
            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
