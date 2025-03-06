using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 2f;
    public float resetDelay = 2f;

    bool falling;
    Rigidbody2D rb;
    //Transform defposition;

    void Start()
    {
        //defposition = gameObject.transform;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        
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
        yield return new WaitForSeconds(resetDelay);
        //Respawn();
    }

    //private void Respawn()
    //{
        //rb.bodyType = RigidbodyType2D.Static;
        //gameObject.transform.position = defposition.position;
    //}
}
