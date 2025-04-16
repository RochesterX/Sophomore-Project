using UnityEngine; using Game; using Music; using Player;
using UnityEngine.InputSystem;

namespace Player
{

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AnimationPlayer))]
public class Punch : MonoBehaviour
{
    public bool cancelable = true;

    [SerializeField] private BoxCollider2D hurtbox;

    InputActionAsset actions;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    private void Update() // Executes punch when 'punch' is pressed
    {
        if (actions.FindAction("Punch").WasPressedThisFrame())
        {
            if (!cancelable) return;
            ExecutePunch();
        }
    }

    private void ExecutePunch() // Triggers punch animation
    {
        GetComponent<AnimationPlayer>().Punch();
        DisableCancellation();
        GetComponent<PlayerMovement>().maxSpeedOverride = 1f; // Slows player down when punching
    }

    public void EnableHurtbox()
    {
        if (hurtbox != null) hurtbox.enabled = true;
    }

    public void DisableHurtbox()
    {
        if (hurtbox != null) hurtbox.enabled = false;
    }

    public void DisableCancellation()
    {
        cancelable = false;
    }

    public void EnableCancellation()
    {
        cancelable = true;
    }

    public void ReturnToMaxSpeed() // Resets player speed after punch
    {
        GetComponent<PlayerMovement>().maxSpeedOverride = GetComponent<PlayerMovement>().maxSpeed;
    }
}
}