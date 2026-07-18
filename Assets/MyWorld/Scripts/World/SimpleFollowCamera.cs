using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Simple follow camera for PlayerMotor testing (replace with Cinemachine later).
    /// </summary>
    public class SimpleFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 3.2f, -6f);
        [SerializeField] private float followSharpness = 8f;
        [SerializeField] private float mouseSensitivity = 2.5f;
        [SerializeField] private float minPitch = -20f;
        [SerializeField] private float maxPitch = 55f;

        private float _yaw;
        private float _pitch = 12f;

        private void LateUpdate()
        {
            if (target == null) return;

            _yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            Quaternion rot = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 desired = target.position + rot * offset;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-followSharpness * Time.deltaTime));
            transform.LookAt(target.position + Vector3.up * 1.4f);
        }

        public void SetTarget(Transform t) => target = t;
    }
}
