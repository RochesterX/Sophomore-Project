using UnityEngine;

public class TeleportPlatform : MonoBehaviour
{
    public Vector2 teleportPoint;
    //public bool teleportPosition = false;
    public string teleportTag;
    public string playerTag = "Player";


    public bool isPlatform = true;

    //private void Start()
    //{
        //if (teleportPosition)
        //{
            //teleportPoint = transform.position;
        //}
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlatform)
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
        else
        {
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
