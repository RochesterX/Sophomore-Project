using System.Collections;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 2f;
    public float resetDelay = 4f;

    bool falling;
    Rigidbody2D rb;
    //Vector3 defposition;

    void Start()
    {
        //defposition = transform.position;
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

    //only resets the object script is attached to, need to fix so platform will reset with fall trigger object
    //private void Respawn()
    //{
        //falling = false;
        //rb.bodyType = RigidbodyType2D.Static;
        //transform.position = defposition;
    //}
}
