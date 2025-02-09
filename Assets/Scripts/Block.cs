using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Block : MonoBehaviour
{
    public bool blocking = false;
    private InputActionAsset actions;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    private void Update()
    {
        InputAction blockAction = actions.FindAction("Block");

        if (blockAction.ReadValue<float>() == 1f)
        {
            blocking = true;
        }
        else
        {
            blocking = false;
        }

        GetComponent<AnimationPlayer>().block = blocking;
    }
}
