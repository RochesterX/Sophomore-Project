using UnityEngine;
using UnityEngine.InputSystem;

public class PongPaddleBehavior : MonoBehaviour
{
    public float speed = 5f;
    public bool isPlayer1 = true;
    
    private InputAction move;
    private Rigidbody2D rb;

    private void Start()
    {
        move = isPlayer1 ? InputSystem.actions.FindAction("Player 1 Move") : InputSystem.actions.FindAction("Player 2 Move");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direction = move.ReadValue<Vector2>().y;
        if (transform.position.y >= 4 && direction > 0 || transform.position.y <= -4 && direction < 0)
        {
            direction = 0;
        }
        rb.linearVelocityY = direction * speed * Time.fixedDeltaTime;
    }
}
