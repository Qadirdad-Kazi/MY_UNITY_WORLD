using UnityEngine;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Motorcycle / bike controller (2 WheelColliders + auto-lean).
    /// Setup: Rigidbody mass ~200–280, WheelCollider front + rear, visuals optional.
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
        [SerializeField] private float leanStrength = 18f;
        [SerializeField] private Transform centerOfMass;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (centerOfMass != null)
                _rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
            else
                _rb.centerOfMass = new Vector3(0f, -0.35f, 0.1f);
            enabled = false;
        }

        private void FixedUpdate()
        {
            if (!IsPlayerDriving) return;

            float speedKmh = _rb.linearVelocity.magnitude * 3.6f;
            float throttle = Vertical;
            float steer = Horizontal * maxSteerAngle;
            float motor = speedKmh < maxSpeedKmh ? throttle * motorTorque : 0f;
            float brake = Mathf.Abs(throttle) < 0.05f ? brakeTorque * 0.3f : 0f;

            if (wheelFront != null) wheelFront.steerAngle = steer;
            if (wheelRear != null)
            {
                wheelRear.motorTorque = motor;
                wheelRear.brakeTorque = brake;
            }
            if (wheelFront != null) wheelFront.brakeTorque = brake * 0.5f;

            // Simple lean for feel
            float lean = -Horizontal * leanStrength * Mathf.Clamp01(speedKmh / 40f);
            Quaternion leanRot = Quaternion.Euler(0f, 0f, lean);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward, leanRot * Vector3.up), Time.fixedDeltaTime * 4f);

            UpdateVisual(wheelFront, visualFront);
            UpdateVisual(wheelRear, visualRear);
        }

        private static void UpdateVisual(WheelCollider col, Transform visual)
        {
            if (col == null || visual == null) return;
            col.GetWorldPose(out Vector3 pos, out Quaternion rot);
            visual.SetPositionAndRotation(pos, rot);
        }

        public override void SetPlayerDriving(bool driving)
        {
            base.SetPlayerDriving(driving);
            if (!driving && wheelRear != null)
            {
                wheelRear.motorTorque = 0f;
                wheelRear.brakeTorque = brakeTorque;
            }
        }
    }
}
