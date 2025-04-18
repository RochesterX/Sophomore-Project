using System.Collections;
using TMPro;
using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class handles the player's movement, including walking, jumping, and animations.
    /// It also manages input, physics, and interactions with the ground.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(AnimationPlayer))]
    [RequireComponent(typeof(Punch))]
    public class PlayerMovement : MonoBehaviour
    {
        // --- Public Fields ---

        /// <summary>
        /// Layers considered as ground for the player.
        /// </summary>
        [Header("Ground Layers")]
        public LayerMask ground;

        /// <summary>
        /// Reference to the player's UI text displaying player index.
        /// </summary>
        public TextMeshProUGUI playerText;

        [Header("Movement")]

        /// <summary>
        /// Base walk speed of the player.
        /// </summary>
        public float walkSpeed;

        /// <summary>
        /// Multiplier applied to walk speed.
        /// </summary>
        public float walkSpeedFactor = 1f;

        /// <summary>
        /// Maximum allowed horizontal speed for the player.
        /// </summary>
        public float maxSpeed = 5f;

        /// <summary>
        /// Runtime override for the maximum speed.
        /// </summary>
        public float maxSpeedOverride;

        /// <summary>
        /// Multiplier for slowing down the player when exceeding max speed.
        /// </summary>
        public float slowdownMultiplier = 10f;

        /// <summary>
        /// Current value of the horizontal movement axis.
        /// </summary>
        public float virtualAxisX;

        /// <summary>
        /// Current value of the jump button (pressed or not).
        /// </summary>
        public float virtualButtonJump;

        /// <summary>
        /// Value of the jump button in the previous frame.
        /// </summary>
        public float virtualButtonJumpLastFrame;

        /// <summary>
        /// Multiplier applied when turning around to adjust speed.
        /// </summary>
        public float turnaroundMultiplier = 2;

        /// <summary>
        /// Smoothing factor for walking movement.
        /// </summary>
        public float walkSmooth;

        /// <summary>
        /// Time in seconds to reach full speed from rest.
        /// </summary>
        public float secondsToFullSpeed;

        /// <summary>
        /// Force applied when jumping.
        /// </summary>
        public float jumpSpeed;

        /// <summary>
        /// Time window after leaving ground where jump is still allowed (coyote time).
        /// </summary>
        public float coyoteTime;

        /// <summary>
        /// Time window after pressing jump where jump is still buffered.
        /// </summary>
        public float jumpLenience;

        /// <summary>
        /// Minimum time before the player can be declared as not jumping.
        /// </summary>
        public float timeUnableToBeDeclaredNotJumping = 0.1f;

        /// <summary>
        /// Distance to check below the player for ground detection.
        /// </summary>
        public float groundCheckDistance;

        // --- Private Fields ---

        /// <summary>
        /// Reference to the Rigidbody2D component.
        /// </summary>
        private Rigidbody2D body;

        /// <summary>
        /// Reference to the BoxCollider2D component.
        /// </summary>
        private BoxCollider2D collide;

        /// <summary>
        /// Reference to the PlayerInput component.
        /// </summary>
        private PlayerInput input;

        /// <summary>
        /// Reference to the AnimationPlayer component.
        /// </summary>
        private AnimationPlayer animationPlayer;

        /// <summary>
        /// Reference to the Punch component.
        /// </summary>
        private Punch punch;

        /// <summary>
        /// Reference to the Damageable component.
        /// </summary>
        private Damageable damageable;

        /// <summary>
        /// Indicates if the jump input is still valid for buffered jumps.
        /// </summary>
        private bool jumpInputStillValid = false;

        /// <summary>
        /// Indicates if the player can be declared as not jumping.
        /// </summary>
        private bool canBeDeclaredNotJumping = true;

        /// <summary>
        /// Indicates if jump physics should be applied this frame.
        /// </summary>
        private bool jumpPhysics;

        /// <summary>
        /// Indicates if the player is currently jumping.
        /// </summary>
        private bool jumping;

        /// <summary>
        /// The last time the jump button was pressed.
        /// </summary>
        private float lastTimeJumpPressed;

        /// <summary>
        /// The last time the player was on the ground.
        /// </summary>
        private float lastTimeOnGround;

        /// <summary>
        /// The player's position in the previous frame.
        /// </summary>
        private Vector3 positionLastFrame;

        // --- Unity Methods ---

        /// <summary>
        /// Initializes player components and sets up initial values.
        /// </summary>
        void Start()
        {
            maxSpeedOverride = maxSpeed;
            GetComponent<RespawnOnTriggerEnter>().spawnPoint = transform.position;

            body = GetComponent<Rigidbody2D>();
            collide = GetComponent<BoxCollider2D>();
            input = GetComponent<PlayerInput>();
            animationPlayer = GetComponent<AnimationPlayer>();
            punch = GetComponent<Punch>();
            damageable = GetComponent<Damageable>();

            playerText.text = input.playerIndex.ToString();
        }

        /// <summary>
        /// Handles per-frame updates for player movement and jump input.
        /// </summary>
        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.gameOver) maxSpeed = 1f;
            UpdateVirtualAxis();
            if (damageable.dying) return;
            Jump();
        }

        /// <summary>
        /// Handles physics-based updates for jumping and horizontal movement.
        /// </summary>
        private void FixedUpdate()
        {
            JumpPhysics();
            HorizontalMovement();
            Land();
        }

        /// <summary>
        /// Handles late frame updates, such as animation.
        /// </summary>
        private void LateUpdate()
        {
            Animate();
        }

        // --- Movement Methods ---

        /// <summary>
        /// Updates the player's animation state based on movement and grounded status.
        /// </summary>
        private void Animate()
        {
            if (!IsPhysicallyGrounded())
                animationPlayer.SetState(AnimationPlayer.AnimationState.Jump);
            else
            {
                if (Mathf.Abs(virtualAxisX) >= 0.05f)
                    animationPlayer.SetState(GameManager.Instance.gameOver ? AnimationPlayer.AnimationState.Walk : AnimationPlayer.AnimationState.Run);
                else
                    animationPlayer.SetState(AnimationPlayer.AnimationState.Idle);
            }

            if (virtualAxisX < -0.01f)
                animationPlayer.backwards = true;
            else if (virtualAxisX > 0.01f)
                animationPlayer.backwards = false;
        }

        /// <summary>
        /// Handles logic for landing and stopping the jump state when grounded.
        /// </summary>
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

        /// <summary>
        /// Handles jump input and determines if a jump should be triggered.
        /// </summary>
        private void Jump()
        {
            if (virtualButtonJumpLastFrame == 1f)
            {
                jumpInputStillValid = true;
                lastTimeJumpPressed = Time.time;
            }

            bool isBasicallyGrounded = IsBasicallyGrounded();
            if ((virtualButtonJumpLastFrame == 1f && isBasicallyGrounded && jumping == false)
                || (jumpInputStillValid && Time.time - lastTimeJumpPressed <= jumpLenience && IsPhysicallyGrounded()))
            {
                AudioManager.Instance.PlaySound("Jump");
                jumpPhysics = true;
                jumping = true;
                jumpInputStillValid = false;
                StartCoroutine(NotJumpingDelay());
            }
        }

        /// <summary>
        /// Applies jump physics and forces to the player.
        /// </summary>
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

        /// <summary>
        /// Coroutine that delays the ability to declare the player as not jumping.
        /// </summary>
        private IEnumerator NotJumpingDelay()
        {
            canBeDeclaredNotJumping = false;
            yield return new WaitUntil(() => !IsBasicallyGrounded());
            canBeDeclaredNotJumping = true;
        }

        /// <summary>
        /// Handles horizontal movement, including acceleration, deceleration, and blocking.
        /// </summary>
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

        /// <summary>
        /// Updates the virtual axis and button values from input actions.
        /// </summary>
        private void UpdateVirtualAxis()
        {
            virtualButtonJump = input.actions.FindAction("Action").ReadValue<float>();
            virtualButtonJumpLastFrame = input.actions.FindAction("Action").WasPressedThisFrame() ? 1 : 0;

            virtualAxisX = input.actions.FindAction("Move").ReadValue<Vector2>().x;
            return;
        }

        /// <summary>
        /// Checks if the player is considered grounded, including coyote time.
        /// </summary>
        /// <returns>True if the player is basically grounded, otherwise false.</returns>
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

        /// <summary>
        /// Checks if the player is physically touching the ground using raycasts.
        /// </summary>
        /// <returns>True if the player is physically grounded, otherwise false.</returns>
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

        /// <summary>
        /// Gets a point on the BoxCollider2D based on horizontal and vertical multipliers.
        /// </summary>
        /// <param name="boxCollider2D">The BoxCollider2D to use.</param>
        /// <param name="horizontal">Horizontal offset (-1 for left, 1 for right, 0 for center).</param>
        /// <param name="vertical">Vertical offset (-1 for bottom, 1 for top, 0 for center).</param>
        /// <returns>The calculated point in world space.</returns>
        public Vector2 GetPointInBoxCollider(BoxCollider2D boxCollider2D, float horizontal, float vertical)
        {
            return new Vector2
            (
                boxCollider2D.bounds.center.x + (horizontal * boxCollider2D.bounds.extents.x),
                boxCollider2D.bounds.center.y + (vertical * boxCollider2D.bounds.extents.y)
            );
        }

        /// <summary>
        /// Stops the player's velocity if grounded, removing inertia.
        /// </summary>
        public void StopVelocity()
        {
            if (IsPhysicallyGrounded()) body.linearVelocity = Vector2.zero;
        }
    }
}
