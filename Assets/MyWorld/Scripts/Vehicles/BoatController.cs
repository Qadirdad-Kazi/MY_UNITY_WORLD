using UnityEngine;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Simple boat for lakes/coast. Keep Y locked-ish with buoyancy spring.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : VehicleControllerBase
    {
        [SerializeField] private float moveForce = 18f;
        [SerializeField] private float turnTorque = 12f;
        [SerializeField] private float waterLevelY = 10f;
        [SerializeField] private float buoyancy = 25f;
        [SerializeField] private float waterDrag = 1.5f;
        [SerializeField] private Transform centerOfMass;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (centerOfMass != null)
                _rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);

            // Dynamic Rigidbodies cannot use concave MeshColliders.
            foreach (var mc in GetComponentsInChildren<MeshCollider>(true))
            {
                if (!mc.convex)
                    mc.convex = true;
            }

            enabled = false;
        }

        private void FixedUpdate()
        {
            // Always apply buoyancy so boat floats even when empty
            float depth = waterLevelY - transform.position.y;
            if (depth > 0f)
            {
                _rb.AddForce(Vector3.up * (depth * buoyancy), ForceMode.Acceleration);
                _rb.linearDamping = waterDrag;
            }
            else
            {
                _rb.linearDamping = 0.2f;
            }

            if (!IsPlayerDriving) return;

            _rb.AddForce(transform.forward * (Vertical * moveForce), ForceMode.Acceleration);
            _rb.AddTorque(Vector3.up * (Horizontal * turnTorque), ForceMode.Acceleration);
        }
    }
}
