using UnityEngine; using Game; using Music; using Player;
using UnityEngine.InputSystem;
namespace Player
{

[RequireComponent(typeof(PlayerInput))]
public class Block : MonoBehaviour
{
    public bool blocking = false;
    private InputActionAsset actions;
    private float blockPressTime = 0f;
    [SerializeField] private float parryThreshold = 0.2f; // Time for successful parry
    private bool isParrying = false;

    private void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }

    private void Update() // Player blocks when "block" is pressed
    {
        InputAction blockAction = actions.FindAction("Block");
        if (blockAction.ReadValue<float>() == 1f)
        {
            if (!blocking)
            {
                blockPressTime = Time.time; // Start parry timer
            }
            blocking = true;
        }
        else
        {
            if (blocking) // Successful parry if blocked in time
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
}