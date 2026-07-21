using UnityEngine;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Motorcycle / bike (2 WheelColliders). Stays upright when parked (kickstand);
    /// soft balance assist while riding so it does not tip over on Play.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BikeController : VehicleControllerBase
    {
        [SerializeField] private WheelCollider wheelFront;
        [SerializeField] private WheelCollider wheelRear;
        [SerializeField] private Transform visualFront;
        [SerializeField] private Transform visualRear;

        [SerializeField] private float motorTorque = 900f;
        [SerializeField] private float brakeTorque = 1400f;
        [SerializeField] private float maxSteerAngle = 28f;
        [SerializeField] private float maxSpeedKmh = 100f;
        [SerializeField] private float leanStrength = 12f;
        [Tooltip("Enable if W goes backward (mesh faces opposite of transform.forward).")]
        [SerializeField] private bool invertThrottle;
        [Tooltip("Enable if A steers right / D steers left.")]
        [SerializeField] private bool invertSteer;
        [Tooltip("Enable if bike leans left when you turn right (common with mirrored / -Z meshes).")]
        [SerializeField] private bool invertLean;
        [SerializeField] private Transform centerOfMass;

        public override bool InvertDriveForward => invertThrottle;

        [Header("Stability (stops falling on Play)")]
        [Tooltip("Freeze tip-over while nobody is riding (like a kickstand).")]
        [SerializeField] private bool kickstandWhenParked = true;
        [Tooltip("How hard to push the bike upright while riding.")]
        [SerializeField] private float uprightStrength = 35f;
        [SerializeField] private float uprightDamping = 8f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            if (centerOfMass != null)
                _rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
            else
                _rb.centerOfMass = new Vector3(0f, -0.25f, 0.05f);

            ApplyKickstand(true);
            enabled = false;
        }

        private void FixedUpdate()
        {
            if (!IsPlayerDriving) return;

            float speedKmh = _rb.linearVelocity.magnitude * 3.6f;
            float throttle = invertThrottle ? -GameInput.Vertical : GameInput.Vertical;
            float steerInput = invertSteer ? -GameInput.Horizontal : GameInput.Horizontal;
            float steer = steerInput * maxSteerAngle;
            float motor = speedKmh < maxSpeedKmh ? throttle * motorTorque : 0f;
            float brake = Mathf.Abs(throttle) < 0.05f ? brakeTorque * 0.3f : 0f;

            if (wheelFront != null) wheelFront.steerAngle = steer;
            if (wheelRear != null)
            {
                wheelRear.motorTorque = motor;
                wheelRear.brakeTorque = brake;
            }
            if (wheelFront != null) wheelFront.brakeTorque = brake * 0.5f;

            StabilizeUpright(steerInput, speedKmh);

            UpdateVisual(wheelFront, visualFront);
            UpdateVisual(wheelRear, visualRear);
        }

        private void StabilizeUpright(float steerInput, float speedKmh)
        {
            // Desired lean only while turning at speed; otherwise stay upright
            float leanSign = invertLean ? 1f : -1f;
            float leanAngle = leanSign * steerInput * leanStrength * Mathf.Clamp01(speedKmh / 50f);
            Quaternion targetRot = Quaternion.Euler(0f, transform.eulerAngles.y, leanAngle);

            Vector3 currentUp = transform.up;
            Vector3 desiredUp = targetRot * Vector3.up;
            Vector3 axis = Vector3.Cross(currentUp, desiredUp);
            float angle = Vector3.Angle(currentUp, desiredUp);

            if (angle > 0.5f)
                _rb.AddTorque(axis.normalized * (angle * uprightStrength), ForceMode.Acceleration);

            // Dampen tip spin
            Vector3 localAng = transform.InverseTransformDirection(_rb.angularVelocity);
            localAng.x *= Mathf.Clamp01(1f - uprightDamping * Time.fixedDeltaTime);
            localAng.z *= Mathf.Clamp01(1f - uprightDamping * Time.fixedDeltaTime);
            _rb.angularVelocity = transform.TransformDirection(localAng);
        }

        private void ApplyKickstand(bool parked)
        {
            if (_rb == null) return;

            if (kickstandWhenParked && parked)
            {
                // Straighten, then freeze tip axes so it cannot fall on Play
                Vector3 e = transform.eulerAngles;
                transform.rotation = Quaternion.Euler(0f, e.y, 0f);
                _rb.angularVelocity = Vector3.zero;
                _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.None;
            }
        }

        private static void UpdateVisual(WheelCollider col, Transform visual)
        {
            if (col == null || visual == null) return;
            col.GetWorldPose(out Vector3 pos, out Quaternion rot);
            visual.SetPositionAndRotation(pos, rot);
        }

        public override void SetPlayerDriving(bool driving)
        {
            if (driving)
                ApplyKickstand(false);

            base.SetPlayerDriving(driving);

            if (!driving)
            {
                if (wheelRear != null)
                {
                    wheelRear.motorTorque = 0f;
                    wheelRear.brakeTorque = brakeTorque;
                }
                if (wheelFront != null)
                {
                    wheelFront.motorTorque = 0f;
                    wheelFront.steerAngle = 0f;
                    wheelFront.brakeTorque = brakeTorque * 0.5f;
                }
                ApplyKickstand(true);
            }
        }
    }
}
