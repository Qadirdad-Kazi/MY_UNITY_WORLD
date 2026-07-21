using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Core;
using MyWorld.World;

namespace MyWorld.Player
{
    /// <summary>
    /// Swim when inside a WaterVolume trigger. Add to PlayerArmature.
    /// Space = up · Left Ctrl / C = dive · WASD = move · Shift = faster.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerSwim : MonoBehaviour
    {
        [Header("Swim")]
        [SerializeField] private float swimSpeed = 3.2f;
        [SerializeField] private float sprintSwimSpeed = 5f;
        [SerializeField] private float verticalSpeed = 2.8f;
        [Tooltip("How hard we push the player back to the surface when not diving.")]
        [SerializeField] private float surfaceFloatStrength = 14f;
        [Tooltip("Feet/pivot depth below surface while floating (≈ 0.85–1.1 for Starter Assets).")]
        [SerializeField] private float floatDepth = 0.95f;
        [SerializeField] private float rotationSharpness = 10f;
        [SerializeField] private float waterDrag = 4f;
        [Tooltip("Max sink below float line before we hard-correct (stops drowning).")]
        [SerializeField] private float maxSinkBelowFloat = 0.35f;

        [Header("Refs")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator animator;

        private CharacterController _cc;
        private PlayerMotor _motor;
        private Behaviour _thirdPerson;
        private readonly System.Collections.Generic.HashSet<WaterVolume> _volumes = new();
        private Vector3 _velocity;
        private int _speedHash;
        private bool _hasSpeedParam;
        private bool _swimAllowed = true;

        public bool IsSwimming { get; private set; }

        public void SetSwimAllowed(bool allowed)
        {
            _swimAllowed = allowed;
            if (!allowed && IsSwimming)
                EndSwim();
        }

        private WaterVolume CurrentWater
        {
            get
            {
                foreach (var w in _volumes)
                    if (w != null) return w;
                return null;
            }
        }

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _motor = GetComponent<PlayerMotor>();
            CacheThirdPerson();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
            if (animator == null) animator = GetComponentInChildren<Animator>();

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                _speedHash = Animator.StringToHash("Speed");
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

        private void CacheThirdPerson()
        {
            foreach (var b in GetComponents<Behaviour>())
            {
                if (b != null && b.GetType().Name == "ThirdPersonController")
                {
                    _thirdPerson = b;
                    break;
                }
            }
        }

        private void Update()
        {
            if (!IsSwimming || !_swimAllowed) return;
            var water = CurrentWater;
            if (water == null)
            {
                EndSwim();
                return;
            }

            if (_cc == null) return;
            if (!_cc.enabled) _cc.enabled = true;

            // Keep ThirdPersonController enabled for mouse look; its Update skips when IsSwimming
            if (_motor != null) _motor.SetMotorEnabled(false);

            float surface = water.SurfaceY;
            float targetFloatY = surface - floatDepth;

            float h = GameInput.Horizontal;
            float v = GameInput.Vertical;

            Vector3 camF = cameraTransform != null ? cameraTransform.forward : transform.forward;
            Vector3 camR = cameraTransform != null ? cameraTransform.right : transform.right;
            camF.y = 0f;
            camR.y = 0f;
            if (camF.sqrMagnitude > 0.001f) camF.Normalize();
            if (camR.sqrMagnitude > 0.001f) camR.Normalize();

            Vector3 wish = Vector3.zero;
            if (h * h + v * v > 0.01f)
            {
                wish = (camF * v + camR * h);
                if (wish.sqrMagnitude > 0.001f)
                {
                    wish.Normalize();
                    Quaternion look = Quaternion.LookRotation(wish, Vector3.up);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        look,
                        1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));
                }
            }

            float speed = GameInput.SprintHeld ? sprintSwimSpeed : swimSpeed;
            Vector3 move = wish * speed;

            bool diving = GameInput.KeyHeld(Key.LeftCtrl) || GameInput.KeyHeld(Key.C);
            bool rising = GameInput.KeyHeld(Key.Space);
            float up = 0f;
            if (rising) up += verticalSpeed;
            if (diving) up -= verticalSpeed;

            if (!diving && !rising)
            {
                float dy = targetFloatY - transform.position.y;
                // Strong float — clamp upward harder so we don't drown
                up += Mathf.Clamp(dy * surfaceFloatStrength, -verticalSpeed * 0.5f, verticalSpeed * 1.5f);
            }

            // Hard rescue if sunk too deep
            if (!diving && transform.position.y < targetFloatY - maxSinkBelowFloat)
            {
                Vector3 p = transform.position;
                p.y = Mathf.Lerp(p.y, targetFloatY, 0.35f);
                bool was = _cc.enabled;
                _cc.enabled = false;
                transform.position = p;
                _cc.enabled = was;
                _velocity.y = Mathf.Max(_velocity.y, 1.5f);
            }

            move.y = up;
            _velocity = Vector3.Lerp(_velocity, move, 1f - Mathf.Exp(-waterDrag * Time.deltaTime));
            _cc.Move(_velocity * Time.deltaTime);

            // Cap at surface (head stays in water, not flying out)
            if (transform.position.y > surface - 0.2f)
            {
                Vector3 p = transform.position;
                p.y = surface - 0.2f;
                bool was = _cc.enabled;
                _cc.enabled = false;
                transform.position = p;
                _cc.enabled = was;
                if (_velocity.y > 0f) _velocity.y = 0f;
            }

            if (_hasSpeedParam && animator != null)
                animator.SetFloat(_speedHash, new Vector2(_velocity.x, _velocity.z).magnitude);
        }

        public void EnterWater(WaterVolume volume)
        {
            if (!_swimAllowed || volume == null) return;
            _volumes.Add(volume);
            if (!IsSwimming)
                BeginSwim();
        }

        public void ExitWater(WaterVolume volume)
        {
            if (volume != null) _volumes.Remove(volume);
            _volumes.RemoveWhere(v => v == null);
            if (_volumes.Count == 0)
                EndSwim();
        }

        private void BeginSwim()
        {
            IsSwimming = true;
            _velocity = Vector3.zero;
            if (_motor != null) _motor.SetMotorEnabled(false);
            if (_thirdPerson == null) CacheThirdPerson();
            // Do not disable ThirdPersonController — LateUpdate handles camera look; Update skips when swimming
            if (_cc != null && !_cc.enabled) _cc.enabled = true;

            // Snap up toward surface immediately so gravity can't pull through first frames
            var water = CurrentWater;
            if (water != null && _cc != null)
            {
                float target = water.SurfaceY - floatDepth;
                if (transform.position.y < target)
                {
                    Vector3 p = transform.position;
                    p.y = target;
                    bool was = _cc.enabled;
                    _cc.enabled = false;
                    transform.position = p;
                    _cc.enabled = was;
                }
            }
        }

        private void EndSwim()
        {
            IsSwimming = false;
            _volumes.Clear();
            _velocity = Vector3.zero;
            if (_motor != null) _motor.SetMotorEnabled(true);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!IsSwimming) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.4f);
        }
#endif
    }
}
