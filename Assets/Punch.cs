using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AnimationPlayer))]
public class Punch : MonoBehaviour
{
    InputActionAsset actions;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    private void Update()
    {
        if (actions.FindAction("Punch").ReadValue<float>() == 1f)
        {
            GetComponent<AnimationPlayer>().Punch();
        }
    }
}
