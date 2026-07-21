using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Plants / bushes that break when a vehicle (or fast object) drives through.
    /// Use a TRIGGER collider so the car is not blocked — solid colliders stop the car.
    /// </summary>
    public class BreakableFoliage : MonoBehaviour
    {
        public enum BreakMode
        {
            Hide,           // Disable renderers (fastest)
            SwapMesh,       // Show a broken mesh child
            PhysicsDebris   // Enable Rigidbody pieces and fling them
        }

        [Header("Break")]
        [SerializeField] private BreakMode mode = BreakMode.Hide;
        [SerializeField] private float minSpeedKmh = 5f;
        [Tooltip("Also break when the player walks through (usually OFF for bushes).")]
        [SerializeField] private bool breakOnPlayer;
        [SerializeField] private float playerMinSpeed = 2f;

        [Header("What to hide / swap")]
        [SerializeField] private GameObject intactVisual;
        [SerializeField] private GameObject brokenVisual;
        [SerializeField] private Rigidbody[] debrisBodies;
        [SerializeField] private float debrisForce = 4f;
        [SerializeField] private float debrisUpForce = 2f;

        [Header("Optional FX")]
        [SerializeField] private ParticleSystem breakParticles;
        [SerializeField] private AudioClip breakSound;
        [SerializeField] [Range(0f, 1f)] private float breakVolume = 0.6f;

        [Header("Collider")]
        [Tooltip("If ON, forces all colliders on this object to Is Trigger so cars pass through.")]
        [SerializeField] private bool forceTriggerColliders = true;

        private bool _broken;
        private AudioSource _audio;

        private void Awake()
        {
            if (intactVisual == null) intactVisual = gameObject;
            if (forceTriggerColliders)
            {
                foreach (var col in GetComponentsInChildren<Collider>(true))
                    col.isTrigger = true;
            }

            if (brokenVisual != null) brokenVisual.SetActive(false);
            if (debrisBodies != null)
            {
                foreach (var rb in debrisBodies)
                {
                    if (rb == null) continue;
                    rb.isKinematic = true;
                    rb.gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other) => TryBreak(other.attachedRigidbody, other.transform);
        private void OnTriggerStay(Collider other) => TryBreak(other.attachedRigidbody, other.transform);
        private void OnCollisionEnter(Collision collision) => TryBreak(collision.rigidbody, collision.transform);

        private void TryBreak(Rigidbody rb, Transform other)
        {
            if (_broken || other == null) return;

            // Vehicles
            if (rb != null)
            {
                bool isVehicle = rb.GetComponentInParent<MyWorld.Vehicles.VehicleControllerBase>() != null
                    || other.GetComponentInParent<MyWorld.Vehicles.VehicleControllerBase>() != null;
                if (isVehicle)
                {
                    float kmh = rb.linearVelocity.magnitude * 3.6f;
                    if (kmh >= minSpeedKmh || rb.linearVelocity.sqrMagnitude > 0.5f)
                        Break(rb.linearVelocity);
                    return;
                }
            }

            // Optional player
            if (breakOnPlayer && other.CompareTag("Player"))
            {
                var cc = other.GetComponentInParent<CharacterController>();
                float speed = cc != null ? cc.velocity.magnitude : 0f;
                if (speed >= playerMinSpeed)
                    Break(other.forward * speed);
            }
        }

        public void Break(Vector3 hitVelocity)
        {
            if (_broken) return;
            _broken = true;

            if (breakParticles != null)
            {
                breakParticles.transform.SetParent(null);
                breakParticles.Play();
            }

            if (breakSound != null)
            {
                if (_audio == null) _audio = gameObject.AddComponent<AudioSource>();
                _audio.playOnAwake = false;
                _audio.spatialBlend = 1f;
                _audio.PlayOneShot(breakSound, breakVolume);
            }

            switch (mode)
            {
                case BreakMode.SwapMesh:
                    if (intactVisual != null) intactVisual.SetActive(false);
                    if (brokenVisual != null) brokenVisual.SetActive(true);
                    break;

                case BreakMode.PhysicsDebris:
                    if (intactVisual != null && intactVisual != gameObject)
                        intactVisual.SetActive(false);
                    else
                        HideRenderers(intactVisual != null ? intactVisual : gameObject);

                    if (debrisBodies != null)
                    {
                        foreach (var rb in debrisBodies)
                        {
                            if (rb == null) continue;
                            rb.gameObject.SetActive(true);
                            rb.isKinematic = false;
                            Vector3 dir = hitVelocity.sqrMagnitude > 0.01f
                                ? hitVelocity.normalized
                                : Random.onUnitSphere;
                            rb.AddForce(dir * debrisForce + Vector3.up * debrisUpForce, ForceMode.Impulse);
                            rb.AddTorque(Random.insideUnitSphere * debrisForce, ForceMode.Impulse);
                        }
                    }
                    break;

                default: // Hide
                    HideRenderers(intactVisual != null ? intactVisual : gameObject);
                    foreach (var col in GetComponentsInChildren<Collider>(true))
                        col.enabled = false;
                    break;
            }
        }

        private static void HideRenderers(GameObject root)
        {
            if (root == null) return;
            foreach (var r in root.GetComponentsInChildren<Renderer>(true))
                r.enabled = false;
        }
    }
}
