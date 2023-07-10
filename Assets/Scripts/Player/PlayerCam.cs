using UnityEngine;

namespace Coder100.Corridors
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField]
        private float mouseSens = 100f;

        [SerializeField]
        private Transform playerBody;

        [SerializeField]
        private float maxX = 90;

        private Quaternion camDefault;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            camDefault = transform.localRotation;
        }

        private void Update()
        {
            RotateY();
            RotateX();

            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        private void RotateX()
        {
            float horiz = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            Quaternion rotation = Quaternion.AngleAxis(horiz, Vector3.up);
            playerBody.localRotation *= rotation;
        }

        private void RotateY()
        {
            float vert = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
            Quaternion rotation = Quaternion.AngleAxis(vert, Vector3.left);
            Quaternion delta = transform.localRotation * rotation;
            if (Quaternion.Angle(camDefault, delta) < maxX) transform.localRotation = delta;
        }
    }
}