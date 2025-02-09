using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AnimationPlayer))]
[RequireComponent(typeof(Punch))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Layers")]
    public LayerMask ground;

    public TextMeshProUGUI playerText;

    [Header("Movement")]
    public float walkSpeed;
    public float walkSpeedFactor = 1f;
    public float maxSpeed = 5f;
    public float maxSpeedOverride;
    public float slowdownMultiplier = 10f;
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
    private PlayerInput input;
    private AnimationPlayer animationPlayer;
    private Punch punch;

    private bool jumpInputStillValid = false;
    private float lastTimeJumpPressed;

    private bool canBeDeclaredNotJumping = true;

    private bool jumpPhysics;

    private bool jumping;

    private float lastTimeOnGround;

    private Vector3 positionLastFrame;

    void Start()
    {
        maxSpeedOverride = maxSpeed;
        GetComponent<RespawnOnTriggerEnter>().spawnPoint = transform.position;

        body = GetComponent<Rigidbody2D>();
        collide = GetComponent<BoxCollider2D>();
        input = GetComponent<PlayerInput>();
        animationPlayer = GetComponent<AnimationPlayer>();
        punch = GetComponent<Punch>();

        playerText.text = input.playerIndex.ToString();
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

    private void LateUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        if (!IsPhysicallyGrounded())
            animationPlayer.SetState(AnimationPlayer.AnimationState.Jump);
        else
        {
            if (Mathf.Abs(body.linearVelocityX) >= 0.1f)
                animationPlayer.SetState(AnimationPlayer.AnimationState.Run);
            else
                animationPlayer.SetState(AnimationPlayer.AnimationState.Idle);
        }

        if (body.linearVelocityX < -0.1f)
            animationPlayer.backwards = true;
        else if (body.linearVelocityX > 0.1f)
            animationPlayer.backwards = false;
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
        //if (!punch.cancelable) return;

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
            if (!GetComponent<Block>().blocking)
            {
                if (body.linearVelocity.y < 0 || !IsPhysicallyGrounded()) body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
                body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                if (Mathf.Abs(body.linearVelocityX) > maxSpeed)
                {
                    body.linearVelocity = new Vector2(Mathf.Sign(body.linearVelocityX) * maxSpeed, body.linearVelocity.y);
                }
            }
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
        float temporaryMax = IsPhysicallyGrounded() ? maxSpeedOverride : Mathf.Infinity;
        float temporarySlowdown = IsPhysicallyGrounded() ? slowdownMultiplier : 1;

        if (!GetComponent<Block>().blocking && (Mathf.Abs(body.linearVelocityX) <= maxSpeed || Mathf.Sign(body.linearVelocityX) != Mathf.Sign(virtualAxisX)))
        {
            body.AddForce(new Vector2(virtualAxisX * walkSpeed * walkSpeedFactor, 0), ForceMode2D.Force);
        }

        if (Mathf.Abs(body.linearVelocityX) >= temporaryMax)
        {
            //body.linearVelocity = new Vector2(Mathf.Sign(body.linearVelocityX) * temporaryMax, body.linearVelocity.y);
            body.AddForce(new Vector2(-Mathf.Sign(body.linearVelocityX) * (Mathf.Abs(body.linearVelocityX) - temporaryMax) * temporarySlowdown, 0));
        }

        if (transform.position == positionLastFrame && (input.actions.FindAction("Move").ReadValue<Vector2>().x == 0))
        {
            virtualAxisX = 0;
        }

        if (GetComponent<Block>().blocking)
        {
            body.AddForce(new Vector2(-body.linearVelocityX * 0.8f, 0), ForceMode2D.Force);
        }

        positionLastFrame = transform.position;
    }

    private void UpdateVirtualAxis()
    {
        virtualButtonJump = input.actions.FindAction("Action").ReadValue<float>();
        virtualButtonJumpLastFrame = input.actions.FindAction("Action").WasPressedThisFrame() ? 1 : 0;

        virtualAxisX = input.actions.FindAction("Move").ReadValue<Vector2>().x;
        return;
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

    public void StopVelocity()
    {
        if (IsPhysicallyGrounded()) body.linearVelocity = Vector2.zero;
    }
}
