using System.Collections;
using UnityEngine;

public class PongBallBehavior : MonoBehaviour
{
    public static Vector2 score;

    public float speed = 400f;
    public float speedIncrement = 0.1f;
    public float resetDelay = 1;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Reset());
    }

    public void IncreaseSpeed()
    {
        rb.AddForce(rb.linearVelocity.normalized * speedIncrement, ForceMode2D.Force);
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);

        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(resetDelay);

        int direction = Random.Range(0, 2);
        rb.AddForce(new Vector2(direction == 0 ? speed : -speed, 0), ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pong Goal"))
        {
            if (other.transform.position.x > 0) score.x++;
            else score.y++;
            StartCoroutine(Reset());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Pong Paddle"))
        {
            IncreaseSpeed();
        }
    }
}
