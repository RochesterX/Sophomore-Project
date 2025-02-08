using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
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

    private void Update()
    {
        if (actions.FindAction("Punch").WasPressedThisFrame())
        {
            if (!cancelable) return;
            ExecutePunch();
        }
    }

    private void ExecutePunch()
    {
        print("Execution");
        GetComponent<AnimationPlayer>().Punch();
        DisableCancellation();
        GetComponent<PlayerMovement>().maxSpeedOverride = 1f;
    }

    public void EnableHurtbox()
    {
        hurtbox.enabled = true;
    }

    public void DisableHurtbox()
    {
        hurtbox.enabled = false;
    }

    public void DisableCancellation()
    {
        cancelable = false;
    }

    public void EnableCancellation()
    {
        cancelable = true;
    }

    public void ReturnToMaxSpeed()
    {
        GetComponent<PlayerMovement>().maxSpeedOverride = GetComponent<PlayerMovement>().maxSpeed;
    }
}
