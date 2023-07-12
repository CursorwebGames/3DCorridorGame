using UnityEngine;

namespace Coder100.Corridors
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController controller;

        [SerializeField]
        private float speed = 12;

        [SerializeField]
        private float gravity = Physics.gravity.y;

        [SerializeField]
        private float jumpHeight = 3;

        private Vector3 velocity;

        // Update is called once per frame
        private void Update()
        {
            // gravity
            if (controller.isGrounded)
            {
                if (velocity.y < 0)
                {
                    velocity.y = 0;
                }
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            // movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z + velocity;
            controller.Move(speed * Time.deltaTime * move);

            // jump
            if (Input.GetButton("Jump") && controller.isGrounded)
            {
                velocity.y = jumpHeight;
            }
        }
    }
}