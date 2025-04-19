using UnityEngine;
using Game;
using Music;
using Player;

namespace Player
{
    /// <summary>
    /// This class manages the player's animations, including setting animation states,
    /// handling directional changes, and triggering specific animations like punching.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        /// <summary>
        /// Represents the different animation states the player can be in.
        /// </summary>
        public enum AnimationState
        {
            /// <summary>
            /// The idle state, when the player is not moving.
            /// </summary>
            Idle,

            /// <summary>
            /// The running state, when the player is moving quickly.
            /// </summary>
            Run,

            /// <summary>
            /// The jumping state, when the player is in the air.
            /// </summary>
            Jump,

            /// <summary>
            /// The walking state, when the player is moving slowly.
            /// </summary>
            Walk
        }

        /// <summary>
        /// The current animation state of the player.
        /// </summary>
        public AnimationState state;

        /// <summary>
        /// Indicates whether the player is facing backwards.
        /// </summary>
        public bool backwards;

        /// <summary>
        /// Indicates whether the player is currently blocking.
        /// </summary>
        public bool block = false;

        /// <summary>
        /// The animation clip to play when the script starts.
        /// </summary>
        public AnimationClip clip;

        /// <summary>
        /// Reference to the Animator component that controls the player's animations.
        /// </summary>
        private Animator animator;

        /// <summary>
        /// Initializes the Animator component and plays the specified animation clip.
        /// </summary>
        private void Start()
        {
            animator = GetComponent<Animator>();

            // Play the initial animation clip
            if (clip != null)
            {
                animator.Play(clip.name);
            }
        }

        /// <summary>
        /// Updates the player's animation state and direction every frame.
        /// </summary>
        private void LateUpdate()
        {
            // Set the animation state in the Animator
            animator.SetInteger("state", (int)state);

            // Adjust the player's scale to reflect their facing direction
            transform.localScale = new Vector3(
                Mathf.Sign(backwards ? -1 : 1) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );

            // Update the blocking state in the Animator
            animator.SetBool("block", block);
        }

        /// <summary>
        /// Sets the player's animation state.
        /// </summary>
        /// <param name="state">The new animation state to set.</param>
        public void SetState(AnimationState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Triggers the punch animation.
        /// </summary>
        public void Punch()
        {
            animator.SetTrigger("punch");
            if (!GetComponent<PlayerMovement>().IsPhysicallyGrounded())
            {
                AudioManager.Instance.PlaySound("Air Punch");
            }
        }
    }
}

