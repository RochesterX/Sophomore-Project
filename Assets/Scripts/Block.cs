using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Block : MonoBehaviour
{
    public bool blocking = false;
    private InputActionAsset actions;
    private float blockPressTime = 0f;
    private float parryThreshold = 0.2f;
    private bool isParrying = false;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    private void Update()
    {
        InputAction blockAction = actions.FindAction("Block");
        if (blockAction.ReadValue<float>() == 1f)
        {
            if (!blocking)
            {
                blockPressTime = Time.time;
            }
            blocking = true;
        }
        else
        {
            if (blocking)
            {
                float pressDuration = Time.time - blockPressTime;
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
        GetComponent<AnimationPlayer>().block = blocking;
    }

    private void Parry()
    {
        isParrying = true;
    }

    public bool IsParrying()
    {
        return isParrying;
    }
}
