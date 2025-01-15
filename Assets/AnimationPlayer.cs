using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    public AnimationClip clip;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(clip.name);
    }
}
