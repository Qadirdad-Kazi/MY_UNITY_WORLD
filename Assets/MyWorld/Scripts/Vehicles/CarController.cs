using UnityEngine;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// WheelCollider car with tunable suspension for a more realistic feel.
    /// Setup: Rigidbody + 4 WheelColliders + VehicleSeat + VehicleEnterExit.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : VehicleControllerBase
    {
        [Header("Wheels")]
        [SerializeField] private WheelCollider wheelFL;
        [SerializeField] private WheelCollider wheelFR;
        [SerializeField] private WheelCollider wheelRL;
        [SerializeField] private WheelCollider wheelRR;
        [SerializeField] private Transform visualFL;
        [SerializeField] private Transform visualFR;
        [SerializeField] private Transform visualRL;
        [SerializeField] private Transform visualRR;

        [Header("Power")]
        [SerializeField] private float motorTorque = 1600f;
        [SerializeField] private float brakeTorque = 3500f;
        [SerializeField] private float handbrakeTorque = 5000f;
        [SerializeField] private float maxSteerAngle = 30f;
        [SerializeField] private float maxSpeedKmh = 120f;
        [SerializeField] private bool allWheelDrive;
        [SerializeField] private bool useSpaceAsHandbrake = true;
        [Tooltip("Enable if W goes backward (mesh faces opposite of transform.forward).")]
        [SerializeField] private bool invertThrottle;
        [Tooltip("Enable if A steers right / D steers left.")]
        [SerializeField] private bool invertSteer;

        [Header("Suspension (applied on Awake)")]
        [SerializeField] private float suspensionDistance = 0.25f;
        [SerializeField] private float spring = 35000f;
        [SerializeField] private float damper = 4500f;
        [SerializeField] private float targetPosition = 0.5f;
        [SerializeField] private float wheelRadius = 0.35f;
        [SerializeField] private float wheelMass = 30f;

        [Header("Grip (raise these if the car slides like ice)")]
        [Tooltip("Forward grip. Slippery ≈ 1.0 · Normal ≈ 1.8 · Sports sticky ≈ 2.4")]
        [SerializeField] private float forwardStiffness = 1.8f;
        [Tooltip("Side grip (turns). Slippery ≈ 1.0 · Normal ≈ 1.7 · Sports sharp ≈ 2.5")]
        [SerializeField] private float sidewaysStiffness = 1.7f;

        [Header("Feel")]
        [SerializeField] private Transform centerOfMass;
        [SerializeField] private float downforce = 50f;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            if (centerOfMass != null)
                _rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
            else
                _rb.centerOfMass = new Vector3(0f, -0.55f, 0.1f);

            ConfigureWheel(wheelFL);
            ConfigureWheel(wheelFR);
            ConfigureWheel(wheelRL);
            ConfigureWheel(wheelRR);

            enabled = false;
        }

        private void ConfigureWheel(WheelCollider wheel)
        {
            if (wheel == null) return;
            wheel.suspensionDistance = suspensionDistance;
            wheel.radius = wheelRadius;
            wheel.mass = wheelMass;

            JointSpring js = wheel.suspensionSpring;
            js.spring = spring;
            js.damper = damper;
            js.targetPosition = targetPosition;
            wheel.suspensionSpring = js;

            WheelFrictionCurve fwd = wheel.forwardFriction;
            fwd.extremumSlip = 0.4f;
            fwd.extremumValue = 1f;
            fwd.asymptoteSlip = 0.8f;
            fwd.asymptoteValue = 0.75f;
            fwd.stiffness = forwardStiffness;
            wheel.forwardFriction = fwd;

            WheelFrictionCurve side = wheel.sidewaysFriction;
            side.extremumSlip = 0.3f;
            side.extremumValue = 1f;
            side.asymptoteSlip = 0.6f;
            side.asymptoteValue = 0.8f;
            side.stiffness = sidewaysStiffness;
            wheel.sidewaysFriction = side;
        }

        private void FixedUpdate()
        {
            // Re-apply grip every physics step so Inspector tweaks work while playing
            ConfigureWheel(wheelFL);
            ConfigureWheel(wheelFR);
            ConfigureWheel(wheelRL);
            ConfigureWheel(wheelRR);

            if (!IsPlayerDriving) return;

            float speedKmh = _rb.linearVelocity.magnitude * 3.6f;
            float throttle = invertThrottle ? -Vertical : Vertical;
            float steerInput = invertSteer ? -Horizontal : Horizontal;
            float steer = steerInput * maxSteerAngle * Mathf.Lerp(1f, 0.45f, speedKmh / maxSpeedKmh);

            float motor = 0f;
            float brake = 0f;
            bool handbrake = useSpaceAsHandbrake && GameInput.KeyHeld(UnityEngine.InputSystem.Key.Space);

            if (speedKmh < maxSpeedKmh)
                motor = throttle * motorTorque;

            if (throttle < 0f && Vector3.Dot(_rb.linearVelocity, transform.forward) > 1.5f)
            {
                motor = 0f;
                brake = brakeTorque;
            }
            else if (Mathf.Abs(throttle) < 0.05f)
            {
                brake = brakeTorque * 0.2f;
            }

            float rearBrake = handbrake ? handbrakeTorque : brake;

            wheelFL.steerAngle = steer;
            wheelFR.steerAngle = steer;

            ApplyDrive(wheelFL, motor, brake, allWheelDrive);
            ApplyDrive(wheelFR, motor, brake, allWheelDrive);
            ApplyDrive(wheelRL, motor, rearBrake, true);
            ApplyDrive(wheelRR, motor, rearBrake, true);

            // Light aero downforce for high-speed stability
            _rb.AddForce(-transform.up * (downforce * _rb.linearVelocity.magnitude));

            UpdateVisual(wheelFL, visualFL);
            UpdateVisual(wheelFR, visualFR);
            UpdateVisual(wheelRL, visualRL);
            UpdateVisual(wheelRR, visualRR);
        }

        private static void ApplyDrive(WheelCollider wheel, float motor, float brake, bool driven)
        {
            if (wheel == null) return;
            wheel.motorTorque = driven ? motor : 0f;
            wheel.brakeTorque = brake;
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
            if (!driving)
            {
                ApplyDrive(wheelFL, 0f, brakeTorque, true);
                ApplyDrive(wheelFR, 0f, brakeTorque, true);
                ApplyDrive(wheelRL, 0f, brakeTorque, true);
                ApplyDrive(wheelRR, 0f, brakeTorque, true);
            }
        }
    }
}
