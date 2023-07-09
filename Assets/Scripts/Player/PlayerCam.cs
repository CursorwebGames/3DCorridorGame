using UnityEngine;

namespace Coder100.Corridors
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField]
        private float mouseSens = 100f;

        [SerializeField]
        private Transform playerBody;

        private float xRotation = 0;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

            // y rotation (horiz)
            playerBody.Rotate(Vector3.up * mouseX);

            // x rotation (vert)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}