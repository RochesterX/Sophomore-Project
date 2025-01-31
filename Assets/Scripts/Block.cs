using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animation))]
public class Block : MonoBehaviour
{
    public bool cancelable = true;
    [SerializeField] private BoxCollider2D blockArea;

    private InputActionAsset actions;
    private Animation animationComponent;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
        animationComponent = GetComponent<Animation>();
    }

    private void Update()
    {
        var blockAction = actions.FindAction("Block");

        if (blockAction.triggered)
        {
            Debug.Log("Block action triggered!");

            if (!cancelable) return;

            animationComponent.Play("Block");

            DisableCancellation();
            ActivateBlockArea();
        }
        else
        {
            DeactivateBlockArea();
        }
    }

    public void ActivateBlockArea()
    {
        blockArea.enabled = true;
    }

    public void DeactivateBlockArea()
    {
        blockArea.enabled = false;
    }

    public void DisableCancellation()
    {
        cancelable = false;
    }

    public void EnableCancellation()
    {
        cancelable = true;
    }

    public bool IsBlocking()
    {
        return blockArea.enabled;
    }
}
