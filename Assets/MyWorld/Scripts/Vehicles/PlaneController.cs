using UnityEngine;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Arcade-realistic plane. Rigidbody + simple lift/thrust.
    /// Controls: W/S thrust, A/D yaw, Mouse Y / Vertical pitch optional via keys I/K or R/F.
    /// Default: Vertical = pitch assist, Horizontal = roll/yaw mix.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlaneController : VehicleControllerBase
    {
        [SerializeField] private float thrustForce = 40f;
        [SerializeField] private float maxSpeed = 80f;
        [SerializeField] private float liftFactor = 0.35f;
        [SerializeField] private float pitchTorque = 30f;
        [SerializeField] private float yawTorque = 20f;
        [SerializeField] private float rollTorque = 35f;
        [SerializeField] private float autoLevel = 0.4f;
        [SerializeField] private Transform centerOfMass;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = true;
            if (centerOfMass != null)
                _rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
            enabled = false;
        }

        private void FixedUpdate()
        {
            if (!IsPlayerDriving) return;

            float throttle = Mathf.Clamp01(Vertical > 0f ? Vertical : 0f);
            float pitchInput = Input.GetAxis("Vertical"); // nose up/down while also throttle — remap if needed
            // Cleaner: use Q/E roll, A/D yaw, W/S pitch when airborne — keep simple for beginners:
            float yaw = Horizontal;
            float pitch = Input.GetKey(KeyCode.R) ? 1f : Input.GetKey(KeyCode.F) ? -1f : -pitchInput * 0.35f;
            float roll = 0f;
            if (Input.GetKey(KeyCode.Q)) roll = 1f;
            if (Input.GetKey(KeyCode.E)) roll = -1f;

            if (_rb.linearVelocity.magnitude < maxSpeed)
                _rb.AddForce(transform.forward * (throttle * thrustForce), ForceMode.Acceleration);

            // Lift grows with forward airspeed
            float speed = Vector3.Dot(_rb.linearVelocity, transform.forward);
            float lift = Mathf.Max(0f, speed) * liftFactor;
            _rb.AddForce(transform.up * lift, ForceMode.Acceleration);

            _rb.AddRelativeTorque(new Vector3(pitch * pitchTorque, yaw * yawTorque, roll * rollTorque), ForceMode.Acceleration);

            // Mild auto-level
            Vector3 predictedUp = Quaternion.AngleAxis(_rb.angularVelocity.z * Mathf.Rad2Deg * autoLevel, transform.forward) * Vector3.up;
            Vector3 torqueVector = Vector3.Cross(transform.up, predictedUp);
            _rb.AddTorque(torqueVector * autoLevel * 2f, ForceMode.Acceleration);
        }
    }
}
