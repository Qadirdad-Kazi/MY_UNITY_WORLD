using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Follows the player (PlayerCameraRoot) while walking.
    /// While driving, follows the vehicle with a chase cam that swings
    /// to the front when reversing so you can see where you are going.
    /// Put this on MainCamera (same component you already use).
    /// </summary>
    public class CameraFollowTarget : MonoBehaviour
    {
        [Header("On foot")]
        [SerializeField] private Transform target;
        [Tooltip("Local offset from PlayerCameraRoot. Z negative = behind player.")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 0.5f, -4f);
        [SerializeField] private float positionSharpness = 12f;
        [SerializeField] private bool matchRotation = true;

        [Header("Driving chase cam")]
        [SerializeField] private Vector3 driveOffset = new Vector3(0f, 2.8f, -7.5f);
        [Tooltip("When reversing: camera sits in front of the car looking back at it.")]
        [SerializeField] private Vector3 reverseOffset = new Vector3(0f, 2.8f, 6.5f);
        [SerializeField] private float drivePositionSharpness = 8f;
        [SerializeField] private float reverseBlendSpeed = 3.5f;
        [SerializeField] private float reverseSpeedThreshold = 1.2f;
        [SerializeField] private float lookHeight = 1.2f;

        private bool _driving;
        private Transform _vehicle;
        private Rigidbody _vehicleRb;
        private float _reverseBlend;

        private void LateUpdate()
        {
            if (_driving && _vehicle != null)
            {
                UpdateDrivingCamera();
                return;
            }

            if (target == null) return;

            Vector3 desired = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                1f - Mathf.Exp(-positionSharpness * Time.deltaTime));

            if (matchRotation)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    target.rotation,
                    1f - Mathf.Exp(-positionSharpness * Time.deltaTime));
        }

        private void UpdateDrivingCamera()
        {
            float along = 0f;
            if (_vehicleRb != null)
                along = Vector3.Dot(_vehicleRb.linearVelocity, _vehicle.forward);

            bool wantReverse = along < -reverseSpeedThreshold;
            float targetBlend = wantReverse ? 1f : 0f;
            _reverseBlend = Mathf.MoveTowards(
                _reverseBlend,
                targetBlend,
                reverseBlendSpeed * Time.deltaTime);

            Vector3 localOffset = Vector3.Lerp(driveOffset, reverseOffset, _reverseBlend);
            Vector3 desiredPos = _vehicle.TransformPoint(localOffset);
            Vector3 lookAt = _vehicle.position + Vector3.up * lookHeight;

            float t = 1f - Mathf.Exp(-drivePositionSharpness * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPos, t);

            Quaternion desiredRot = Quaternion.LookRotation((lookAt - transform.position).normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, t);
        }

        public void SetTarget(Transform t) => target = t;

        /// <summary>Call when the player enters a vehicle.</summary>
        public void BeginDriving(Transform vehicle, Rigidbody vehicleBody = null)
        {
            _driving = true;
            _vehicle = vehicle;
            _vehicleRb = vehicleBody != null ? vehicleBody : vehicle.GetComponent<Rigidbody>();
            _reverseBlend = 0f;
        }

        /// <summary>Call when the player exits a vehicle.</summary>
        public void EndDriving()
        {
            _driving = false;
            _vehicle = null;
            _vehicleRb = null;
            _reverseBlend = 0f;
        }

        public bool IsDriving => _driving;
    }
}
