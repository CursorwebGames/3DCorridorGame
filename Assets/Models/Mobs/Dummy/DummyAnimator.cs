using UnityEngine;

namespace Coder100.Corridors
{
    public class DummyAnimator : MonoBehaviour
    {
        public Animator animator;

        private void Update()
        {
            animator.SetBool("isPunching", true);
        }
    }
}