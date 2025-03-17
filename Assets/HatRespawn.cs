using UnityEngine;

public class HatRespawn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platformer Hazard"))
        {
            transform.position = GameManager.Instance.hatSpawnPosition;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}
