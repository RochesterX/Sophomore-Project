using UnityEngine;
using UnityEngine.InputSystem;

public class Trevor : MonoBehaviour
{
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 direction = InputSystem.actions.FindAction("Player 1 Move").ReadValue<Vector2>();
        transform.position += speed * (Vector3)direction * Time.deltaTime;
    }
}
