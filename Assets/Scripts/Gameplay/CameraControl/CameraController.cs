using UnityEngine;

namespace Gameplay.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float movingSpeed = 15;
        
        private Vector3 input;

        private void Update() => GatherInput();
        private void LateUpdate() => CameraMove();
        private void GatherInput() => input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        private void CameraMove()
        {
            Transform camTransform = transform;
            
            Vector3 position = camTransform.right * (input.x * movingSpeed);
            position += camTransform.up * (input.z * movingSpeed);
            
            Vector3 camPos = camTransform.position;
            camPos += position * Time.deltaTime;
            camPos = ClampCameraPos(camPos);
            camTransform.position = camPos;
        }

        private Vector3 ClampCameraPos(Vector3 posToClamp)
        {
            float x = Mathf.Clamp(posToClamp.x, -40, 10);
            float y = Mathf.Clamp(posToClamp.y, 5, 30);
            float z = Mathf.Clamp(posToClamp.z, -40, 30);

            return new Vector3(x, y, z);
        }
    }
}