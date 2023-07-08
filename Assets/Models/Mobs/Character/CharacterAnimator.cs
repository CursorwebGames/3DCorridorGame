using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    // public CharacterController characterController;
    public Animator animator;

    private void Update() {
        animator.SetBool("isPunching", true);
    }
}
