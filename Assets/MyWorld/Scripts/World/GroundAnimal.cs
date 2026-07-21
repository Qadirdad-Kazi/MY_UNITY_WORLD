using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Surface animals (deer, rabbits, dogs, livestock) that walk on terrain.
    /// Uses a ground raycast — no NavMesh required (works on painted terrain).
    /// </summary>
    public class GroundAnimal : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float walkSpeed = 1.8f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] [Range(0f, 1f)] private float runChance = 0.2f;
        [SerializeField] private float turnSpeed = 4f;
        [SerializeField] private float wanderRadius = 18f;
        [SerializeField] private float arriveDistance = 1.2f;
        [SerializeField] private float minIdle = 1.5f;
        [SerializeField] private float maxIdle = 5f;

        [Header("Ground (surface stick)")]
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float groundProbeHeight = 4f;
        [SerializeField] private float groundProbeDistance = 10f;
        [SerializeField] private float heightOffset = 0f;
        [SerializeField] private bool alignToSlope = true;
        [SerializeField] private float slopeAlignSpeed = 6f;

        [Header("Life")]
        [SerializeField] private bool faceMoveDirection = true;
        [SerializeField] private bool randomizeOnStart = true;
        [SerializeField] private Animator animator;
        [SerializeField] private string speedParam = "Speed";

        private Vector3 _home;
        private Vector3 _target;
        private float _idleUntil;
        private float _currentSpeed;
        private int _speedHash;
        private bool _hasSpeedParam;
        private Vector3 _groundNormal = Vector3.up;

        private void Awake()
        {
            if (animator == null) animator = GetComponentInChildren<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                _speedHash = Animator.StringToHash(speedParam);
                foreach (var p in animator.parameters)
                {
                    if (p.nameHash == _speedHash && p.type == AnimatorControllerParameterType.Float)
                    {
                        _hasSpeedParam = true;
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            _home = transform.position;
            if (randomizeOnStart)
            {
                walkSpeed *= Random.Range(0.9f, 1.1f);
                _idleUntil = Time.time + Random.Range(0.2f, 2f);
            }
            SnapToGround(force: true);
            PickNewTarget();
        }

        private void Update()
        {
            if (Time.time < _idleUntil)
            {
                SetAnimSpeed(0f);
                SnapToGround(force: false);
                return;
            }

            Vector3 pos = transform.position;
            Vector3 flatTarget = _target;
            flatTarget.y = pos.y;
            Vector3 to = flatTarget - pos;
            to.y = 0f;

            if (to.sqrMagnitude <= arriveDistance * arriveDistance)
            {
                _idleUntil = Time.time + Random.Range(minIdle, maxIdle);
                _currentSpeed = 0f;
                SetAnimSpeed(0f);
                PickNewTarget();
                return;
            }

            Vector3 dir = to.normalized;
            if (faceMoveDirection && dir.sqrMagnitude > 0.001f)
            {
                Quaternion look = Quaternion.LookRotation(dir, alignToSlope ? _groundNormal : Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    look,
                    1f - Mathf.Exp(-turnSpeed * Time.deltaTime));
            }

            pos += dir * (_currentSpeed * Time.deltaTime);
            transform.position = pos;
            SnapToGround(force: false);
            SetAnimSpeed(_currentSpeed);
        }

        private void PickNewTarget()
        {
            _currentSpeed = Random.value < runChance ? runSpeed : walkSpeed;
            for (int i = 0; i < 10; i++)
            {
                Vector2 r = Random.insideUnitCircle * wanderRadius;
                Vector3 candidate = _home + new Vector3(r.x, 0f, r.y);
                if (SampleGround(candidate, out Vector3 hitPoint, out _))
                {
                    _target = hitPoint;
                    return;
                }
            }
            _target = _home;
            SampleGround(_home, out _target, out _);
        }

        private void SnapToGround(bool force)
        {
            if (!SampleGround(transform.position, out Vector3 hitPoint, out Vector3 normal))
                return;

            _groundNormal = normal;
            Vector3 p = transform.position;
            float targetY = hitPoint.y + heightOffset;
            p.y = force ? targetY : Mathf.Lerp(p.y, targetY, 1f - Mathf.Exp(-12f * Time.deltaTime));
            transform.position = p;

            if (alignToSlope && faceMoveDirection)
            {
                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, normal).normalized;
                if (forward.sqrMagnitude > 0.001f)
                {
                    Quaternion slopeRot = Quaternion.LookRotation(forward, normal);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        slopeRot,
                        1f - Mathf.Exp(-slopeAlignSpeed * Time.deltaTime));
                }
            }
        }

        private bool SampleGround(Vector3 around, out Vector3 hitPoint, out Vector3 normal)
        {
            Vector3 origin = around + Vector3.up * groundProbeHeight;
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundProbeDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                hitPoint = hit.point;
                normal = hit.normal;
                return true;
            }
            hitPoint = around;
            normal = Vector3.up;
            return false;
        }

        private void SetAnimSpeed(float speed)
        {
            if (_hasSpeedParam && animator != null)
                animator.SetFloat(speedParam, speed);
        }

        public void ConfigureRuntime(float walk, float run, float radius)
        {
            walkSpeed = Mathf.Max(0.1f, walk);
            runSpeed = Mathf.Max(walkSpeed, run);
            wanderRadius = Mathf.Max(1f, radius);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 c = Application.isPlaying ? _home : transform.position;
            Gizmos.color = new Color(0.4f, 0.9f, 0.45f, 0.35f);
            Gizmos.DrawWireSphere(c, wanderRadius);
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_target, 0.3f);
            }
        }
#endif
    }
}
