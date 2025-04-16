using UnityEngine; using Game; using Music; using Player;
using UnityEngine.InputSystem;
namespace Archaic{
public class PongPaddleBehavior : MonoBehaviour
{
    public float speed = 5f;
    public bool isPlayer1 = true;
    
    [SerializeField] private InputActionAsset move;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isPlayer1)
        {
            Transform ball = FindFirstObjectByType<PongBallBehavior>().transform;

            RaycastHit2D[] hits = Physics2D.RaycastAll(ball.position, ball.GetComponent<Rigidbody2D>().linearVelocity, Mathf.Infinity, LayerMask.GetMask("Pong Goal"));
            Debug.DrawRay(ball.position, ball.GetComponent<Rigidbody2D>().linearVelocity * 1000f, Color.red);

            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Pong Goal"))
                {
                    print("Hit");
                    float dir = transform.position.y - hit.point.y > 0 ? 1 : -1;

                    rb.linearVelocityY = dir * speed * Time.fixedDeltaTime;
                }
            }

            return;
        }
        float direction = move.FindAction("Move").ReadValue<Vector2>().y;
        if (transform.position.y >= 4 && direction > 0 || transform.position.y <= -4 && direction < 0)
        {
            direction = 0;
        }
        rb.linearVelocityY = direction * speed * Time.fixedDeltaTime;
    }
}
}