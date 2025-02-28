using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 2f;
    public float destroyDelay = 2f;

    bool falling;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (!falling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        falling = true;
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
