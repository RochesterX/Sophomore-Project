using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    public enum AnimationState { Idle, Run, Jump, Walk };
    public AnimationState state;
    public bool backwards;
    public bool block = false;
    public AnimationClip clip;
    private Animator animator;

    private void Start() // Plays the specified animation clip
    {
        animator = GetComponent<Animator>();
        animator.Play(clip.name);
    }

    private void LateUpdate() // Updates the animation state
    {
        animator.SetInteger("state", (int)state);
        transform.localScale = new Vector3(Mathf.Sign(backwards ? -1 : 1) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        animator.SetBool("block", block);
    }

    public void SetState(AnimationState state) // Sets the animation state
    {
        this.state = state;
    }

    public void Punch() // Triggers punch animation
    {
        animator.SetTrigger("punch");
    }
}
