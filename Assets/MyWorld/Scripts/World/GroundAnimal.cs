using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Surface animals that walk, rest, and flee from the player. Optional Animator for legs/body.
    /// </summary>
    public class GroundAnimal : MonoBehaviour
    {
        public enum AnimalState
        {
            Idle,
            Walk,
            Run,
            Rest,
            Flee
        }

        [Header("Move")]
        [SerializeField] private float walkSpeed = 1.8f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float fleeSpeedMultiplier = 1.25f;
        [SerializeField] [Range(0f, 1f)] private float runChance = 0.2f;
        [SerializeField] private float turnSpeed = 4f;
        [SerializeField] private float wanderRadius = 18f;
        [SerializeField] private float arriveDistance = 1.2f;

        [Header("Life cycle (rest / walk)")]
        [SerializeField] private bool enableResting = true;
        [SerializeField] private float minIdle = 1.5f;
        [SerializeField] private float maxIdle = 5f;
        [SerializeField] private Vector2 restDuration = new Vector2(3f, 9f);
        [SerializeField] [Range(0f, 1f)] private float restChance = 0.45f;

        [Header("Player awareness")]
        [SerializeField] private bool reactToPlayer = true;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float noticeDistance = 10f;
        [SerializeField] private float fleeDistance = 18f;
        [SerializeField] private float calmDownTime = 5f;

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

        [Header("Animator (optional — legs / tail / head)")]
        [SerializeField] private Animator animator;
        [SerializeField] private string speedParam = "Speed";
        [SerializeField] private string restingParam = "IsResting";
        [SerializeField] private string fleeingParam = "IsFleeing";

        private Vector3 _home;
        private Vector3 _target;
        private float _stateUntil;
        private float _calmUntil;
        private float _currentSpeed;
        private AnimalState _state = AnimalState.Idle;
        private Transform _player;
        private int _speedHash, _restHash, _fleeHash;
        private bool _hasSpeed, _hasRest, _hasFlee;
        private Vector3 _groundNormal = Vector3.up;

        public AnimalState CurrentState => _state;

        private void Awake()
        {
            if (animator == null) animator = GetComponentInChildren<Animator>();
            CacheAnimParams();
        }

        private void Start()
        {
            _home = transform.position;
            ResolvePlayer();
            if (randomizeOnStart)
            {
                walkSpeed *= Random.Range(0.9f, 1.1f);
                _stateUntil = Time.time + Random.Range(0.2f, 2f);
            }
            SnapToGround(force: true);
            EnterIdle();
            PickNewTarget();
        }

        private void Update()
        {
            UpdateAwareness();

            if (_state == AnimalState.Idle || _state == AnimalState.Rest)
            {
                SetAnim(0f);
                SnapToGround(force: false);
                if (Time.time >= _stateUntil && _state != AnimalState.Flee)
                    BeginMoveOrRest();
                return;
            }

            Vector3 pos = transform.position;
            Vector3 flatTarget = _target;
            flatTarget.y = pos.y;
            Vector3 to = flatTarget - pos;
            to.y = 0f;

            if (to.sqrMagnitude <= arriveDistance * arriveDistance)
            {
                if (_state == AnimalState.Flee && _player != null)
                {
                    Vector3 away = (pos - _player.position);
                    away.y = 0f;
                    if (away.sqrMagnitude < 0.01f) away = Random.insideUnitSphere;
                    away.y = 0f;
                    TrySetTarget(pos + away.normalized * fleeDistance);
                    return;
                }

                EnterIdleOrRest();
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

            float speed = _currentSpeed;
            if (_state == AnimalState.Flee) speed = runSpeed * fleeSpeedMultiplier;

            pos += dir * (speed * Time.deltaTime);
            transform.position = pos;
            SnapToGround(force: false);
            SetAnim(speed);
        }

        private void UpdateAwareness()
        {
            if (!reactToPlayer) return;
            if (_player == null)
            {
                ResolvePlayer();
                if (_player == null) return;
            }

            float dist = Vector3.Distance(transform.position, _player.position);
            if (dist <= noticeDistance)
            {
                EnterFlee();
                _calmUntil = Time.time + calmDownTime;
            }
            else if (_state == AnimalState.Flee && dist > fleeDistance && Time.time >= _calmUntil)
            {
                EnterIdle();
                PickNewTarget();
            }
        }

        private void BeginMoveOrRest()
        {
            // After idle, either walk/run or sometimes go into a longer rest
            if (enableResting && Random.value < restChance * 0.35f)
            {
                EnterRest();
                return;
            }

            _currentSpeed = Random.value < runChance ? runSpeed : walkSpeed;
            _state = _currentSpeed >= runSpeed * 0.95f ? AnimalState.Run : AnimalState.Walk;
            PickNewTarget();
        }

        private void EnterIdleOrRest()
        {
            if (enableResting && Random.value < restChance)
                EnterRest();
            else
                EnterIdle();
        }

        private void EnterIdle()
        {
            _state = AnimalState.Idle;
            _currentSpeed = 0f;
            _stateUntil = Time.time + Random.Range(minIdle, maxIdle);
            SetAnim(0f);
        }

        private void EnterRest()
        {
            _state = AnimalState.Rest;
            _currentSpeed = 0f;
            _stateUntil = Time.time + Random.Range(restDuration.x, restDuration.y);
            SetAnim(0f);
        }

        private void EnterFlee()
        {
            _state = AnimalState.Flee;
            _currentSpeed = runSpeed * fleeSpeedMultiplier;
            if (_player != null)
            {
                Vector3 away = transform.position - _player.position;
                away.y = 0f;
                if (away.sqrMagnitude < 0.01f) away = transform.forward;
                TrySetTarget(transform.position + away.normalized * fleeDistance);
            }
            else
            {
                PickNewTarget();
            }
        }

        private void PickNewTarget()
        {
            if (_state != AnimalState.Flee)
                _currentSpeed = Random.value < runChance ? runSpeed : walkSpeed;

            for (int i = 0; i < 10; i++)
            {
                Vector2 r = Random.insideUnitCircle * wanderRadius;
                Vector3 candidate = _home + new Vector3(r.x, 0f, r.y);
                if (SampleGround(candidate, out Vector3 hitPoint, out _))
                {
                    _target = hitPoint;
                    if (_state != AnimalState.Flee && _state != AnimalState.Idle && _state != AnimalState.Rest)
                        _state = _currentSpeed >= runSpeed * 0.95f ? AnimalState.Run : AnimalState.Walk;
                    return;
                }
            }
            SampleGround(_home, out _target, out _);
        }

        private void TrySetTarget(Vector3 world)
        {
            if (SampleGround(world, out Vector3 hit, out _))
                _target = hit;
            else
                PickNewTarget();
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

        private void ResolvePlayer()
        {
            var go = GameObject.FindGameObjectWithTag(playerTag);
            if (go != null) _player = go.transform;
            else if (Camera.main != null) _player = Camera.main.transform;
        }

        private void CacheAnimParams()
        {
            if (animator == null || animator.runtimeAnimatorController == null) return;
            _speedHash = Animator.StringToHash(speedParam);
            _restHash = Animator.StringToHash(restingParam);
            _fleeHash = Animator.StringToHash(fleeingParam);
            foreach (var p in animator.parameters)
            {
                if (p.nameHash == _speedHash && p.type == AnimatorControllerParameterType.Float) _hasSpeed = true;
                if (p.nameHash == _restHash && p.type == AnimatorControllerParameterType.Bool) _hasRest = true;
                if (p.nameHash == _fleeHash && p.type == AnimatorControllerParameterType.Bool) _hasFlee = true;
            }
        }

        private void SetAnim(float speed)
        {
            if (animator == null) return;
            if (_hasSpeed) animator.SetFloat(_speedHash, speed);
            if (_hasRest) animator.SetBool(_restHash, _state == AnimalState.Rest);
            if (_hasFlee) animator.SetBool(_fleeHash, _state == AnimalState.Flee);
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
            if (reactToPlayer)
            {
                Gizmos.color = new Color(1f, 0.35f, 0.35f, 0.25f);
                Gizmos.DrawWireSphere(transform.position, noticeDistance);
            }
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_target, 0.3f);
            }
        }
#endif
    }
}
