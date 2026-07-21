using UnityEngine;
using UnityEngine.AI;

namespace MyWorld.Npc
{
    /// <summary>
    /// Makes a character walk automatically on a baked NavMesh.
    /// Modes: random wander, waypoint patrol, or idle.
    /// Requires: Nav Mesh Agent + baked NavMesh (see MASTER_GUIDE §16).
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcWander : MonoBehaviour
    {
        public enum MoveMode
        {
            Wander,
            Patrol,
            Idle
        }

        [Header("Mode")]
        [SerializeField] private MoveMode mode = MoveMode.Wander;

        [Header("Wander")]
        [Tooltip("How far from the start position they may roam.")]
        [SerializeField] private float wanderRadius = 12f;
        [SerializeField] private float minWait = 1.5f;
        [SerializeField] private float maxWait = 4.5f;
        [SerializeField] private float arriveDistance = 0.6f;

        [Header("Patrol (optional)")]
        [Tooltip("Empty transforms they walk between in order. Leave empty for Wander.")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private bool loopPatrol = true;
        [SerializeField] private float waitAtWaypoint = 2f;

        [Header("Agent")]
        [SerializeField] private float walkSpeed = 1.4f;
        [SerializeField] private float runSpeed = 3.2f;
        [SerializeField] [Range(0f, 1f)] private float runChance = 0.15f;
        [SerializeField] private float angularSpeed = 120f;
        [SerializeField] private float stoppingDistance = 0.35f;

        [Header("Animator (optional)")]
        [SerializeField] private Animator animator;
        [Tooltip("Float parameter name (often Speed or MoveSpeed).")]
        [SerializeField] private string speedParam = "Speed";
        [SerializeField] private float animSpeedMultiplier = 1f;
        [SerializeField] private bool useIsWalkingBool;
        [SerializeField] private string isWalkingParam = "IsWalking";

        [Header("Avoid player (optional)")]
        [SerializeField] private bool avoidPlayer;
        [SerializeField] private float avoidDistance = 3.5f;
        [SerializeField] private string playerTag = "Player";

        private NavMeshAgent _agent;
        private Vector3 _home;
        private float _waitUntil;
        private int _patrolIndex;
        private Transform _player;
        private int _speedHash;
        private int _walkHash;
        private bool _hasSpeedParam;
        private bool _hasWalkParam;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = walkSpeed;
            _agent.angularSpeed = angularSpeed;
            _agent.stoppingDistance = stoppingDistance;
            _agent.autoBraking = true;

            if (animator == null) animator = GetComponentInChildren<Animator>();
            CacheAnimatorParams();
        }

        private void Start()
        {
            _home = transform.position;
            if (!_agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
                {
                    _agent.Warp(hit.position);
                    _home = hit.position;
                }
                else
                {
                    Debug.LogWarning($"[NpcWander] {name} is not on a NavMesh. Bake NavMesh (MASTER_GUIDE §16).", this);
                    enabled = false;
                    return;
                }
            }

            if (avoidPlayer)
            {
                var p = GameObject.FindGameObjectWithTag(playerTag);
                if (p != null) _player = p.transform;
            }

            _waitUntil = Time.time + Random.Range(0.2f, 1f);
        }

        private void Update()
        {
            if (!_agent.enabled || !_agent.isOnNavMesh) return;

            if (avoidPlayer && _player != null)
            {
                float d = Vector3.Distance(transform.position, _player.position);
                if (d < avoidDistance)
                {
                    Vector3 away = (transform.position - _player.position).normalized;
                    TrySetDestination(transform.position + away * (avoidDistance + 1.5f));
                    UpdateAnimator();
                    return;
                }
            }

            switch (mode)
            {
                case MoveMode.Idle:
                    _agent.isStopped = true;
                    break;
                case MoveMode.Wander:
                    TickWander();
                    break;
                case MoveMode.Patrol:
                    TickPatrol();
                    break;
            }

            UpdateAnimator();
        }

        private void TickWander()
        {
            if (Time.time < _waitUntil) return;

            if (!_agent.pathPending && _agent.remainingDistance <= arriveDistance)
            {
                _waitUntil = Time.time + Random.Range(minWait, maxWait);
                _agent.isStopped = true;
                PickWanderDestination();
            }
        }

        private void TickPatrol()
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                mode = MoveMode.Wander;
                return;
            }

            if (Time.time < _waitUntil) return;

            if (!_agent.hasPath || _agent.remainingDistance <= arriveDistance)
            {
                if (_agent.hasPath && _agent.remainingDistance <= arriveDistance)
                {
                    _waitUntil = Time.time + waitAtWaypoint;
                    _agent.isStopped = true;
                    _patrolIndex++;
                    if (_patrolIndex >= waypoints.Length)
                    {
                        if (!loopPatrol)
                        {
                            mode = MoveMode.Idle;
                            return;
                        }
                        _patrolIndex = 0;
                    }
                }

                Transform wp = waypoints[_patrolIndex];
                if (wp != null)
                    TrySetDestination(wp.position, forceWalk: true);
            }
        }

        private void PickWanderDestination()
        {
            bool run = Random.value < runChance;
            _agent.speed = run ? runSpeed : walkSpeed;

            for (int i = 0; i < 12; i++)
            {
                Vector2 r = Random.insideUnitCircle * wanderRadius;
                Vector3 candidate = _home + new Vector3(r.x, 0f, r.y);
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2.5f, NavMesh.AllAreas))
                {
                    TrySetDestination(hit.position);
                    return;
                }
            }
            // Fallback: stay near home
            if (NavMesh.SamplePosition(_home, out NavMeshHit homeHit, 3f, NavMesh.AllAreas))
                TrySetDestination(homeHit.position);
        }

        private void TrySetDestination(Vector3 worldPos, bool forceWalk = false)
        {
            if (forceWalk) _agent.speed = walkSpeed;
            if (NavMesh.SamplePosition(worldPos, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                _agent.isStopped = false;
                _agent.SetDestination(hit.position);
            }
        }

        private void UpdateAnimator()
        {
            if (animator == null) return;
            float speed = _agent.velocity.magnitude;
            if (_hasSpeedParam)
                animator.SetFloat(speedParam, speed * animSpeedMultiplier);
            if (_hasWalkParam && useIsWalkingBool)
                animator.SetBool(isWalkingParam, speed > 0.15f);
        }

        private void CacheAnimatorParams()
        {
            _hasSpeedParam = false;
            _hasWalkParam = false;
            if (animator == null || animator.runtimeAnimatorController == null) return;

            _speedHash = Animator.StringToHash(speedParam);
            _walkHash = Animator.StringToHash(isWalkingParam);
            foreach (var p in animator.parameters)
            {
                if (p.nameHash == _speedHash && p.type == AnimatorControllerParameterType.Float)
                    _hasSpeedParam = true;
                if (p.nameHash == _walkHash && p.type == AnimatorControllerParameterType.Bool)
                    _hasWalkParam = true;
            }
        }

        public void SetMode(MoveMode m) => mode = m;

        public void SetHomeToCurrent()
        {
            _home = transform.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 c = Application.isPlaying ? _home : transform.position;
            Gizmos.color = new Color(0.2f, 0.85f, 0.45f, 0.35f);
            Gizmos.DrawWireSphere(c, wanderRadius);

            if (waypoints == null) return;
            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;
                Gizmos.DrawSphere(waypoints[i].position, 0.25f);
                if (i + 1 < waypoints.Length && waypoints[i + 1] != null)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
#endif
    }
}
