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
            camTransform.position += position * Time.deltaTime;
        }
    }
}