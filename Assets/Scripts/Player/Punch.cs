using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class handles the punching mechanic for the player, including triggering animations,
    /// enabling and disabling the hurtbox, and managing player speed during a punch.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(AnimationPlayer))]
    public class Punch : MonoBehaviour
    {
        /// <summary>
        /// Determines whether the player can cancel their punch action.
        /// </summary>
        public bool cancelable = true;

        /// <summary>
        /// The hurtbox used to detect collisions with other players or objects during a punch.
        /// </summary>
        [SerializeField] private BoxCollider2D hurtbox;

        /// <summary>
        /// The input actions associated with the player.
        /// </summary>
        private InputActionAsset actions;

        /// <summary>
        /// Initializes the player's input actions.
        /// </summary>
        private void Start()
        {
            actions = GetComponent<PlayerInput>().actions;
        }

        /// <summary>
        /// Checks for punch input every frame and executes the punch if the action is triggered.
        /// </summary>
        private void Update()
        {
            // Executes punch when the "Punch" action is pressed
            if (actions.FindAction("Punch").WasPressedThisFrame())
            {
                if (!cancelable) return; // Prevents punching if the action is not cancelable
                ExecutePunch();
            }
        }

        /// <summary>
        /// Executes the punch action, triggering the punch animation and slowing the player down.
        /// </summary>
        private void ExecutePunch()
        {
            // Trigger the punch animation
            GetComponent<AnimationPlayer>().Punch();

            // Disable the ability to cancel the punch
            DisableCancellation();

            // Temporarily reduce the player's movement speed during the punch
            GetComponent<PlayerMovement>().maxSpeedOverride = 1f;
        }

        /// <summary>
        /// Enables the hurtbox, allowing the punch to interact with other objects.
        /// </summary>
        public void EnableHurtbox()
        {
            if (hurtbox != null) hurtbox.enabled = true;
        }

        /// <summary>
        /// Disables the hurtbox, preventing the punch from interacting with other objects.
        /// </summary>
        public void DisableHurtbox()
        {
            if (hurtbox != null) hurtbox.enabled = false;
        }

        /// <summary>
        /// Disables the ability to cancel the punch action.
        /// </summary>
        public void DisableCancellation()
        {
            cancelable = false;
        }

        /// <summary>
        /// Enables the ability to cancel the punch action.
        /// </summary>
        public void EnableCancellation()
        {
            cancelable = true;
        }

        /// <summary>
        /// Resets the player's movement speed to its maximum value after the punch is complete.
        /// </summary>
        public void ReturnToMaxSpeed()
        {
            GetComponent<PlayerMovement>().maxSpeedOverride = GetComponent<PlayerMovement>().maxSpeed;
        }
    }
}
