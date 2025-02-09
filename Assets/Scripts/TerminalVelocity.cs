using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TerminalVelocity : MonoBehaviour
{
    [SerializeField] private float terminalVelocity = -10f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < terminalVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, terminalVelocity);
        }
    }
}
