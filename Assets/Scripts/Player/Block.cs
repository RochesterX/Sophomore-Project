using UnityEngine;
using Game;
using Music;
using Player;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class handles the player's ability to block and parry incoming attacks.
    /// Blocking reduces damage, while parrying reflects attacks if timed correctly.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class Block : MonoBehaviour
    {
        /// <summary>
        /// Indicates whether the player is currently blocking.
        /// </summary>
        public bool blocking = false;

        /// <summary>
        /// The input actions associated with the player.
        /// </summary>
        private InputActionAsset actions;

        /// <summary>
        /// The time when the block button was pressed.
        /// </summary>
        private float blockPressTime = 0f;

        /// <summary>
        /// The maximum time (in seconds) for a successful parry after pressing the block button.
        /// </summary>
        [SerializeField] private float parryThreshold = 0.2f;

        /// <summary>
        /// Indicates whether the player is currently parrying.
        /// </summary>
        private bool isParrying = false;

        /// <summary>
        /// Initializes the player's input actions.
        /// </summary>
        private void Start()
        {
            actions = GetComponent<PlayerInput>().actions;
        }

        /// <summary>
        /// Updates the player's blocking and parrying state every frame based on input.
        /// </summary>
        private void Update()
        {
            // Get the block action from the input system
            InputAction blockAction = actions.FindAction("Block");

            // Check if the block button is being pressed
            if (blockAction.ReadValue<float>() == 1f)
            {
                if (!blocking)
                {
                    // Start the parry timer when the block button is first pressed
                    blockPressTime = Time.time;
                }
                blocking = true;
            }
            else
            {
                // Handle the release of the block button
                if (blocking)
                {
                    // Calculate how long the block button was held
                    float pressDuration = Time.time - blockPressTime;

                    // If the button was released within the parry threshold, trigger a parry
                    if (pressDuration <= parryThreshold)
                    {
                        Parry();
                    }
                    else
                    {
                        isParrying = false;
                    }
                }
                blocking = false;
            }

            // Update the blocking state in the animation system
            GetComponent<AnimationPlayer>().block = blocking;
        }

        /// <summary>
        /// Activates the parry state, allowing the player to reflect attacks.
        /// </summary>
        private void Parry()
        {
            isParrying = true;
        }

        /// <summary>
        /// Checks if the player is currently parrying.
        /// </summary>
        /// <returns>True if the player is parrying, false otherwise.</returns>
        public bool IsParrying()
        {
            return isParrying;
        }
    }
}