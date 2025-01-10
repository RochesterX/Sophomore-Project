using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public string player;

    [Header("Ground Layers")]
    public LayerMask ground;

    [Header("Movement")]
    public float walkSpeed;
    public float virtualAxisX;
    public float virtualButtonJump;
    public float virtualButtonJumpLastFrame;
    public float turnaroundMultiplier = 2;
    public float walkSmooth;
    public float secondsToFullSpeed;
    public float jumpSpeed;
    public float coyoteTime;
    public float jumpLenience;
    public float timeUnableToBeDeclaredNotJumping = 0.1f;
    public float groundCheckDistance;

    private Rigidbody2D body;
    private BoxCollider2D collide;

    private Vector2 spawnPosition;

    private bool jumpInputStillValid = false;
    private float lastTimeJumpPressed;

    private bool canBeDeclaredNotJumping = true;

    private bool jumpPhysics;

    private bool jumping;

    private float lastTimeOnGround;

    private Vector3 positionLastFrame;

    void Start()
    {
        spawnPosition = transform.position;

        body = GetComponent<Rigidbody2D>();
        collide = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Jump();

        UpdateVirtualAxis();
    }

    private void FixedUpdate()
    {
        JumpPhysics();

        HorizontalMovement();


        Land();
    }

    private void Land()
    {
        if (body.linearVelocity.y >= 0f) return;

        if (IsPhysicallyGrounded())
        {
            if (canBeDeclaredNotJumping)
            {
                jumping = false;
            }
        }
    }

    private void Jump()
    {
        if (virtualButtonJumpLastFrame == 1f)
        {
            jumpInputStillValid = true;
            lastTimeJumpPressed = Time.time;
        }

        bool isBasicallyGrounded = IsBasicallyGrounded();
        if ((virtualButtonJumpLastFrame == 1f && isBasicallyGrounded && jumping == false) // Coyote Jump: Must have jump pressed this frame and be grounded in last time frame and not be actually jumping.
            || (jumpInputStillValid && Time.time - lastTimeJumpPressed <= jumpLenience && IsPhysicallyGrounded())) // Buffered Jump: Must have pressed jump in the last time frame and be jumping
        {
            jumpPhysics = true;
            jumping = true;
            jumpInputStillValid = false;
            StartCoroutine(NotJumpingDelay());
        }
    }

    private void JumpPhysics()
    {
        if (jumpPhysics)
        {
            if (body.linearVelocity.y < 0 || !IsPhysicallyGrounded()) body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
            body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpPhysics = false;
        }

        if (!IsPhysicallyGrounded() && !(virtualButtonJump == 1f))
            body.AddForce(Vector2.down * jumpSpeed);
    }

    private IEnumerator NotJumpingDelay()
    {
        canBeDeclaredNotJumping = false;
        yield return new WaitUntil(() => !IsBasicallyGrounded());
        canBeDeclaredNotJumping = true;
    }

    private void HorizontalMovement()
    {
        body.linearVelocity = new Vector2(virtualAxisX * walkSpeed, body.linearVelocity.y);

        if (transform.position == positionLastFrame && (InputSystem.actions.FindAction($"Player {player} Move").ReadValue<Vector2>().x == 0))
        {
            virtualAxisX = 0;
        }

        positionLastFrame = transform.position;
    }

    private void UpdateVirtualAxis()
    {
        // From https://discussions.unity.com/t/manually-smooth-input-getaxisraw/225141/4
        float basicallyRawAxis = InputSystem.actions.FindAction($"Player {player} Move").ReadValue<Vector2>().x;
        float sensitivity = 3;
        float gravity = 3;
        float time = Time.deltaTime;

        if (basicallyRawAxis != 0)
        {
            virtualAxisX = Mathf.Clamp(virtualAxisX + basicallyRawAxis * sensitivity * time * turnaroundMultiplier, -1f, 1f);
        }
        else
        {
            virtualAxisX = Mathf.Clamp01(Mathf.Abs(virtualAxisX) - gravity * time) * Mathf.Sign(virtualAxisX);
        }

        if ((basicallyRawAxis > 0f && virtualAxisX < 0f) || (basicallyRawAxis < 0f && virtualAxisX > 0f))
        {
            turnaroundMultiplier = 2;
        }
        else
        {
            turnaroundMultiplier = 1;
        }

        virtualButtonJump = InputSystem.actions.FindAction($"Player {player} Action").ReadValue<float>();
        virtualButtonJumpLastFrame = InputSystem.actions.FindAction($"Player {player} Action").WasPressedThisFrame() ? 1 : 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platformer Hazard"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPosition;
        body.linearVelocity = Vector2.zero;
    }

    public bool IsBasicallyGrounded()
    {
        if (IsPhysicallyGrounded())
        {
            lastTimeOnGround = Time.time;
        }

        if (Time.time - lastTimeOnGround < coyoteTime)
        {
            return true;
        }

        return false;
    }

    public bool IsPhysicallyGrounded()
    {
        RaycastHit2D leftCheck = Physics2D.Raycast(GetPointInBoxCollider(collide, -1, -1), Vector2.down, groundCheckDistance, ground);
        RaycastHit2D rightCheck = Physics2D.Raycast(GetPointInBoxCollider(collide, 1, -1), Vector2.down, groundCheckDistance, ground);
        RaycastHit2D midCheck = Physics2D.Raycast(GetPointInBoxCollider(collide, 0, -1), Vector2.down, groundCheckDistance, ground);

        if (leftCheck || rightCheck || midCheck)
        {
            return true;
        }

        return false;
    }

    public Vector2 GetPointInBoxCollider(BoxCollider2D boxCollider2D, float horizontal, float vertical)
    {
        return new Vector2
        (
            boxCollider2D.bounds.center.x + (horizontal * boxCollider2D.bounds.extents.x),
            boxCollider2D.bounds.center.y + (vertical * boxCollider2D.bounds.extents.y)
        );
    }
}
