using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Flying birds / eagles / flies. No NavMesh — place in the sky and play.
    /// Tune speed & radius: flies = small/fast · birds = medium · eagles = large/slow.
    /// </summary>
    public class FlyingAnimal : MonoBehaviour
    {
        public enum FlyStyle
        {
            Wander,
            Orbit,
            Figure8
        }

        [Header("Style")]
        [SerializeField] private FlyStyle style = FlyStyle.Wander;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float turnSpeed = 2.5f;
        [SerializeField] private float bankStrength = 18f;

        [Header("Area (relative to spawn position)")]
        [Tooltip("Horizontal roam distance from start.")]
        [SerializeField] private float wanderRadius = 25f;
        [Tooltip("Lowest altitude offset from spawn Y (can be negative).")]
        [SerializeField] private float heightMinOffset = -2f;
        [Tooltip("Highest altitude offset from spawn Y.")]
        [SerializeField] private float heightMaxOffset = 10f;
        [SerializeField] private float arriveDistance = 2.5f;

        [Header("Orbit / Figure-8")]
        [SerializeField] private float orbitRadius = 18f;
        [SerializeField] private float orbitSeconds = 22f;

        [Header("Life")]
        [SerializeField] private float flapBob = 0.15f;
        [SerializeField] private float flapSpeed = 5f;
        [SerializeField] private bool faceFlightDirection = true;
        [SerializeField] private bool randomizeOnStart = true;

        private Vector3 _home;
        private Vector3 _target;
        private float _phase;
        private float _bobPhase;

        public void ConfigureRuntime(FlyStyle flyStyle, float speed)
        {
            style = flyStyle;
            moveSpeed = Mathf.Max(0.1f, speed);
        }

        private void Start()
        {
            _home = transform.position;
            if (randomizeOnStart)
            {
                _phase = Random.Range(0f, Mathf.PI * 2f);
                _bobPhase = Random.Range(0f, 10f);
                moveSpeed *= Random.Range(0.85f, 1.15f);
            }
            PickNewTarget();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _bobPhase += dt * flapSpeed;
            _phase += dt * (Mathf.PI * 2f / Mathf.Max(0.5f, orbitSeconds));

            Vector3 desired = style switch
            {
                FlyStyle.Orbit => OrbitPoint(),
                FlyStyle.Figure8 => Figure8Point(),
                _ => _target
            };

            if (style == FlyStyle.Wander
                && (transform.position - _target).sqrMagnitude < arriveDistance * arriveDistance)
            {
                PickNewTarget();
                desired = _target;
            }

            Vector3 to = desired - transform.position;
            if (to.sqrMagnitude > 0.0001f)
            {
                Vector3 dir = to.normalized;
                Vector3 next = transform.position + dir * (moveSpeed * dt);

                if (faceFlightDirection)
                {
                    Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                    float yawDelta = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
                    float bank = Mathf.Clamp(-yawDelta * 0.2f, -bankStrength, bankStrength);
                    look *= Quaternion.Euler(0f, 0f, bank);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        look,
                        1f - Mathf.Exp(-turnSpeed * dt));
                }

                // Wing bob
                next.y += Mathf.Sin(_bobPhase) * flapBob * dt * 4f;
                transform.position = ClampPos(next);
            }
        }

        private Vector3 ClampPos(Vector3 p)
        {
            float yMin = _home.y + Mathf.Min(heightMinOffset, heightMaxOffset);
            float yMax = _home.y + Mathf.Max(heightMinOffset, heightMaxOffset);
            p.y = Mathf.Clamp(p.y, yMin, yMax);

            Vector3 flat = p - _home;
            flat.y = 0f;
            float maxR = style == FlyStyle.Wander ? wanderRadius : orbitRadius * 1.35f;
            if (flat.sqrMagnitude > maxR * maxR)
            {
                flat = flat.normalized * maxR;
                p.x = _home.x + flat.x;
                p.z = _home.z + flat.z;
            }
            return p;
        }

        private Vector3 OrbitPoint()
        {
            float x = Mathf.Cos(_phase) * orbitRadius;
            float z = Mathf.Sin(_phase) * orbitRadius;
            float y = Mathf.Sin(_phase * 2f) * Mathf.Abs(heightMaxOffset - heightMinOffset) * 0.2f;
            return ClampPos(_home + new Vector3(x, y, z));
        }

        private Vector3 Figure8Point()
        {
            float t = _phase;
            float x = Mathf.Sin(t) * orbitRadius;
            float z = Mathf.Sin(t) * Mathf.Cos(t) * orbitRadius;
            float y = Mathf.Sin(t * 2f) * 2f;
            return ClampPos(_home + new Vector3(x, y, z));
        }

        private void PickNewTarget()
        {
            Vector2 xz = Random.insideUnitCircle * wanderRadius;
            float yOff = Random.Range(
                Mathf.Min(heightMinOffset, heightMaxOffset),
                Mathf.Max(heightMinOffset, heightMaxOffset));
            _target = ClampPos(_home + new Vector3(xz.x, yOff, xz.y));
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 c = Application.isPlaying ? _home : transform.position;
            float r = style == FlyStyle.Wander ? wanderRadius : orbitRadius;
            Gizmos.color = new Color(0.35f, 0.75f, 1f, 0.35f);
            Gizmos.DrawWireSphere(c, r);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                c + Vector3.up * Mathf.Min(heightMinOffset, heightMaxOffset),
                c + Vector3.up * Mathf.Max(heightMinOffset, heightMaxOffset));
        }
#endif
    }
}
