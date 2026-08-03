using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Flying birds with wander / rest / flee-from-player and optional Animator (wings).
    /// </summary>
    public class FlyingAnimal : MonoBehaviour
    {
        public enum FlyStyle
        {
            Wander,
            Orbit,
            Figure8
        }

        public enum BirdState
        {
            Fly,
            Rest,
            Flee
        }

        [Header("Style")]
        [SerializeField] private FlyStyle style = FlyStyle.Wander;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float fleeSpeedMultiplier = 1.55f;
        [SerializeField] private float restSpeedMultiplier = 0.25f;
        [SerializeField] private float turnSpeed = 2.5f;
        [SerializeField] private float bankStrength = 18f;
        [SerializeField] private float pitchStrength = 25f;

        [Header("Area (relative to spawn position)")]
        [SerializeField] private float wanderRadius = 25f;
        [SerializeField] private float heightMinOffset = -2f;
        [SerializeField] private float heightMaxOffset = 10f;
        [SerializeField] private float arriveDistance = 2.5f;

        [Header("Orbit / Figure-8")]
        [SerializeField] private float orbitRadius = 18f;
        [SerializeField] private float orbitSeconds = 22f;

        [Header("Life cycle (rest / fly)")]
        [SerializeField] private bool enableResting = true;
        [Tooltip("Seconds of flying before a rest is considered.")]
        [SerializeField] private Vector2 flyDuration = new Vector2(8f, 18f);
        [Tooltip("Seconds to rest / circle slowly.")]
        [SerializeField] private Vector2 restDuration = new Vector2(3f, 8f);
        [SerializeField] [Range(0f, 1f)] private float restChance = 0.65f;

        [Header("Player awareness")]
        [SerializeField] private bool reactToPlayer = true;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float noticeDistance = 12f;
        [SerializeField] private float fleeDistance = 22f;
        [SerializeField] private float calmDownTime = 4f;

        [Header("Obstacle avoidance")]
        [SerializeField] private bool avoidObstacles = true;
        [SerializeField] private LayerMask obstacleMask = ~0;
        [SerializeField] private float lookAhead = 8f;
        [SerializeField] private float feelerRadius = 0.35f;
        [SerializeField] private float sideFeelerAngle = 35f;
        [SerializeField] private float avoidWeight = 2.2f;
        [SerializeField] private float groundClearance = 2.5f;
        [SerializeField] private float groundCheckDistance = 40f;

        [Header("Life / feel")]
        [SerializeField] private float flapBob = 0.12f;
        [SerializeField] private float flapSpeed = 6f;
        [SerializeField] private float speedJitter = 0.2f;
        [SerializeField] private bool faceFlightDirection = true;
        [SerializeField] private bool randomizeOnStart = true;
        [SerializeField] private Vector3 visualEulerOffset = Vector3.zero;

        [Header("Animator (optional — wings / body)")]
        [SerializeField] private Animator animator;
        [SerializeField] private string speedParam = "Speed";
        [SerializeField] private string flapParam = "Flap";
        [SerializeField] private string restingParam = "IsResting";
        [SerializeField] private string fleeingParam = "IsFleeing";

        private Vector3 _home;
        private Vector3 _target;
        private Vector3 _velocity;
        private float _phase;
        private float _bobPhase;
        private float _baseSpeed;
        private BirdState _state = BirdState.Fly;
        private float _stateUntil;
        private float _calmUntil;
        private Transform _player;
        private int _speedHash, _flapHash, _restHash, _fleeHash;
        private bool _hasSpeed, _hasFlap, _hasRest, _hasFlee;
        private readonly RaycastHit[] _hits = new RaycastHit[8];

        public BirdState CurrentState => _state;

        public void ConfigureRuntime(FlyStyle flyStyle, float speed)
        {
            style = flyStyle;
            moveSpeed = Mathf.Max(0.1f, speed);
            _baseSpeed = moveSpeed;
        }

        public void ApplyPigeonPreset()
        {
            style = FlyStyle.Wander;
            moveSpeed = 6.5f;
            turnSpeed = 3.2f;
            bankStrength = 22f;
            pitchStrength = 20f;
            wanderRadius = 35f;
            heightMinOffset = 3f;
            heightMaxOffset = 18f;
            arriveDistance = 2.2f;
            lookAhead = 7f;
            feelerRadius = 0.4f;
            groundClearance = 3f;
            flapBob = 0.14f;
            flapSpeed = 7f;
            avoidObstacles = true;
            enableResting = true;
            reactToPlayer = true;
            noticeDistance = 10f;
            fleeDistance = 20f;
            flyDuration = new Vector2(10f, 20f);
            restDuration = new Vector2(4f, 10f);
        }

        private void Awake()
        {
            if (animator == null) animator = GetComponentInChildren<Animator>();
            CacheAnimParams();
        }

        private void Start()
        {
            _home = transform.position;
            _baseSpeed = moveSpeed;
            _velocity = transform.forward * moveSpeed;
            ResolvePlayer();

            if (randomizeOnStart)
            {
                _phase = Random.Range(0f, Mathf.PI * 2f);
                _bobPhase = Random.Range(0f, 10f);
                moveSpeed *= Random.Range(0.9f, 1.1f);
                _baseSpeed = moveSpeed;
            }

            EnterFly();
            PickNewTarget();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _bobPhase += dt * flapSpeed;
            _phase += dt * (Mathf.PI * 2f / Mathf.Max(0.5f, orbitSeconds));

            UpdateAwareness();
            UpdateStateMachine();

            float speedMul = _state switch
            {
                BirdState.Flee => fleeSpeedMultiplier,
                BirdState.Rest => restSpeedMultiplier,
                _ => 1f
            };
            float speedNow = _baseSpeed * speedMul * (1f + Mathf.Sin(_bobPhase * 0.7f) * speedJitter);

            Vector3 desired = GetDesiredPoint();
            Vector3 toTarget = desired - transform.position;
            Vector3 steer = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : transform.forward;

            if (avoidObstacles)
                steer = ApplyAvoidance(steer);

            steer = ApplyGroundClearance(steer).normalized;

            float turn = _state == BirdState.Flee ? turnSpeed * 1.4f : turnSpeed;
            Vector3 velDir = _velocity.sqrMagnitude > 0.01f ? _velocity.normalized : transform.forward;
            _velocity = Vector3.Slerp(velDir, steer, 1f - Mathf.Exp(-turn * dt)) * speedNow;

            Vector3 next = transform.position + _velocity * dt;
            float bob = _state == BirdState.Rest ? flapBob * 0.25f : flapBob;
            next.y += Mathf.Sin(_bobPhase) * bob * dt * 4f;
            next = ClampPos(next);

            if (faceFlightDirection && _velocity.sqrMagnitude > 0.01f)
            {
                Vector3 dir = _velocity.normalized;
                Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                float yawDelta = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
                float bank = Mathf.Clamp(-yawDelta * 0.25f, -bankStrength, bankStrength);
                float pitch = Mathf.Clamp(-dir.y * pitchStrength, -pitchStrength, pitchStrength);
                look *= Quaternion.Euler(pitch, 0f, bank);
                if (visualEulerOffset != Vector3.zero)
                    look *= Quaternion.Euler(visualEulerOffset);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    look,
                    1f - Mathf.Exp(-turn * dt));
            }

            transform.position = next;
            PushAnimator(speedNow);
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
            else if (_state == BirdState.Flee && dist > fleeDistance && Time.time >= _calmUntil)
            {
                EnterFly();
            }
        }

        private void UpdateStateMachine()
        {
            if (_state == BirdState.Flee) return;
            if (!enableResting || style != FlyStyle.Wander) return;

            if (Time.time < _stateUntil) return;

            if (_state == BirdState.Fly)
            {
                if (Random.value <= restChance)
                    EnterRest();
                else
                    EnterFly();
            }
            else if (_state == BirdState.Rest)
            {
                EnterFly();
            }
        }

        private void EnterFly()
        {
            _state = BirdState.Fly;
            _stateUntil = Time.time + Random.Range(flyDuration.x, flyDuration.y);
            PickNewTarget();
        }

        private void EnterRest()
        {
            _state = BirdState.Rest;
            _stateUntil = Time.time + Random.Range(restDuration.x, restDuration.y);
            // Rest = slow lazy circle near current spot
            _target = ClampPos(transform.position + Random.insideUnitSphere * 3f);
        }

        private void EnterFlee()
        {
            if (_state == BirdState.Flee) return;
            _state = BirdState.Flee;
            if (_player != null)
            {
                Vector3 away = (transform.position - _player.position).normalized;
                away.y = 0.35f;
                _target = ClampPos(transform.position + away.normalized * fleeDistance);
            }
            else
            {
                PickNewTarget();
            }
        }

        private Vector3 GetDesiredPoint()
        {
            if (_state == BirdState.Flee || _state == BirdState.Rest)
            {
                if ((_state == BirdState.Flee || style == FlyStyle.Wander)
                    && (transform.position - _target).sqrMagnitude < arriveDistance * arriveDistance)
                {
                    if (_state == BirdState.Flee && _player != null)
                    {
                        Vector3 away = (transform.position - _player.position).normalized;
                        away.y = Random.Range(0.2f, 0.6f);
                        _target = ClampPos(transform.position + away.normalized * fleeDistance);
                    }
                    else if (_state == BirdState.Rest)
                    {
                        _target = ClampPos(transform.position + Random.insideUnitSphere * 4f);
                    }
                }
                return _target;
            }

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

            return desired;
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
            _flapHash = Animator.StringToHash(flapParam);
            _restHash = Animator.StringToHash(restingParam);
            _fleeHash = Animator.StringToHash(fleeingParam);
            foreach (var p in animator.parameters)
            {
                if (p.nameHash == _speedHash && p.type == AnimatorControllerParameterType.Float) _hasSpeed = true;
                if (p.nameHash == _flapHash && p.type == AnimatorControllerParameterType.Float) _hasFlap = true;
                if (p.nameHash == _restHash && p.type == AnimatorControllerParameterType.Bool) _hasRest = true;
                if (p.nameHash == _fleeHash && p.type == AnimatorControllerParameterType.Bool) _hasFlee = true;
            }
        }

        private void PushAnimator(float speedNow)
        {
            if (animator == null) return;
            if (_hasSpeed) animator.SetFloat(_speedHash, speedNow);
            if (_hasFlap)
            {
                float flap = _state == BirdState.Rest ? 0.15f
                    : _state == BirdState.Flee ? 1f
                    : 0.55f + Mathf.Abs(Mathf.Sin(_bobPhase)) * 0.45f;
                animator.SetFloat(_flapHash, flap);
            }
            if (_hasRest) animator.SetBool(_restHash, _state == BirdState.Rest);
            if (_hasFlee) animator.SetBool(_fleeHash, _state == BirdState.Flee);
        }

        private Vector3 ApplyAvoidance(Vector3 steer)
        {
            Vector3 origin = transform.position;
            Vector3 forward = _velocity.sqrMagnitude > 0.01f ? _velocity.normalized : transform.forward;
            if (forward.sqrMagnitude < 0.01f) forward = Vector3.forward;

            Vector3 avoid = Vector3.zero;
            bool blocked = false;

            if (SphereFeel(origin, forward, lookAhead, out RaycastHit hitF))
            {
                avoid += hitF.normal;
                blocked = hitF.distance < lookAhead * 0.45f;
            }

            Vector3 right = Quaternion.AngleAxis(sideFeelerAngle, Vector3.up) * forward;
            Vector3 left = Quaternion.AngleAxis(-sideFeelerAngle, Vector3.up) * forward;
            if (SphereFeel(origin, right, lookAhead * 0.75f, out RaycastHit hitR)) avoid += hitR.normal * 0.7f;
            if (SphereFeel(origin, left, lookAhead * 0.75f, out RaycastHit hitL)) avoid += hitL.normal * 0.7f;
            if (SphereFeel(origin, (forward + Vector3.up * 0.35f).normalized, lookAhead * 0.6f, out RaycastHit hitU))
                avoid += hitU.normal * 0.5f;

            if (avoid.sqrMagnitude > 0.001f)
                steer = (steer + avoid.normalized * avoidWeight).normalized;

            if (blocked && style == FlyStyle.Wander && _state != BirdState.Flee)
                PickNewTargetAwayFrom(forward);

            return steer;
        }

        private Vector3 ApplyGroundClearance(Vector3 steer)
        {
            if (Physics.Raycast(
                    transform.position + Vector3.up * 0.5f,
                    Vector3.down,
                    out RaycastHit ground,
                    groundCheckDistance,
                    obstacleMask,
                    QueryTriggerInteraction.Ignore))
            {
                float clearance = transform.position.y - ground.point.y;
                if (clearance < groundClearance)
                {
                    float push = (groundClearance - clearance) / Mathf.Max(0.1f, groundClearance);
                    steer = (steer + Vector3.up * (push * 2.5f)).normalized;
                }
            }
            return steer;
        }

        private bool SphereFeel(Vector3 origin, Vector3 dir, float dist, out RaycastHit nearest)
        {
            nearest = default;
            int count = Physics.SphereCastNonAlloc(
                origin, feelerRadius, dir.normalized, _hits, dist, obstacleMask, QueryTriggerInteraction.Ignore);
            float best = float.MaxValue;
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                var h = _hits[i];
                if (h.collider == null) continue;
                if (h.collider.transform == transform || h.collider.transform.IsChildOf(transform)) continue;
                if (h.distance < best) { best = h.distance; nearest = h; found = true; }
            }
            return found;
        }

        private Vector3 ClampPos(Vector3 p)
        {
            float yMin = _home.y + Mathf.Min(heightMinOffset, heightMaxOffset);
            float yMax = _home.y + Mathf.Max(heightMinOffset, heightMaxOffset);
            p.y = Mathf.Clamp(p.y, yMin, yMax);

            Vector3 flat = p - _home;
            flat.y = 0f;
            float maxR = style == FlyStyle.Wander ? wanderRadius : orbitRadius * 1.35f;
            if (_state == BirdState.Flee) maxR *= 1.35f;
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

        private void PickNewTargetAwayFrom(Vector3 awayFrom)
        {
            for (int i = 0; i < 6; i++)
            {
                PickNewTarget();
                Vector3 d = _target - transform.position;
                if (d.sqrMagnitude < 1f) continue;
                if (Vector3.Dot(d.normalized, awayFrom) < 0.15f) return;
            }
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

            if (reactToPlayer)
            {
                Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.25f);
                Gizmos.DrawWireSphere(transform.position, noticeDistance);
            }

            if (avoidObstacles)
            {
                Gizmos.color = new Color(1f, 0.55f, 0.2f, 0.7f);
                Vector3 fwd = Application.isPlaying && _velocity.sqrMagnitude > 0.01f
                    ? _velocity.normalized : transform.forward;
                Gizmos.DrawLine(transform.position, transform.position + fwd * lookAhead);
            }
        }
#endif
    }
}
