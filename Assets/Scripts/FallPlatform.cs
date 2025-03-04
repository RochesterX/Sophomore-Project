using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 2f;
    public float resetDelay = 2f;

    bool falling;
    Rigidbody2D rb;
    //Vector3 position;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        //position = rb.transform.position;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!falling/* && collision.gameObject.CompareTag("Player")*/)
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        falling = true;
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        //yield return new WaitForSeconds(resetDelay);
        
    }
}
