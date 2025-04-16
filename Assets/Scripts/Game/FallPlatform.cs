using System.Collections;
using UnityEngine; using Game; using Music; using Player;

namespace Game
{

public class FallPlatform : MonoBehaviour
{
    public float fallDelay = 2f; // Delay before the platform falls
    public float resetDelay = 4f; // Delay before the platform resets

    bool falling;
    Rigidbody2D rb;
    Vector3 defposition;

    void Start()
    {
        defposition = transform.parent.position;
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision) // Makes platform fall when player or another platform touch it
    {
        try
        {
            if (collision.transform.childCount != 0 && !falling && (collision.gameObject.CompareTag("Player") || collision.transform.GetChild(0).TryGetComponent(out FallPlatform _)))
            {
                StartCoroutine(FallAfterDelay());
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error in FallPlatform: " + e.Message);
        }
    }

    private IEnumerator FallAfterDelay() // Sets platform to fall and respawn
    {
        falling = true;
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(resetDelay);
        transform.parent.GetComponent<Animator>().SetTrigger("respawn");
        yield return new WaitForSeconds(0.5f);
        Respawn();
    }

    //only resets the object script is attached to, need to fix so platform will reset with fall trigger object
    // Use transform.parent to get the object it's attatched to
    private void Respawn() // Resets the platform position
    {
        falling = false;
        rb.bodyType = RigidbodyType2D.Static;
        transform.parent.position = defposition;
        transform.parent.rotation = Quaternion.identity;
    }
}
}