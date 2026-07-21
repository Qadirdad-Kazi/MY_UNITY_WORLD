using UnityEngine;
using MyWorld.Core;

namespace MyWorld.Player
{
    /// <summary>
    /// Clean third-person walker using CharacterController + Input System.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float walkSpeed = 4.2f;
        [SerializeField] private float runSpeed = 7.5f;
        [SerializeField] private float rotationSharpness = 12f;
        [SerializeField] private float jumpHeight = 1.2f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float groundedGravity = -2f;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float groundCheckRadius = 0.28f;
        [SerializeField] private Vector3 groundCheckOffset = new Vector3(0f, 0.1f, 0f);

        [Header("Refs")]
        [SerializeField] private Transform cameraTransform;

        private CharacterController _controller;
        private Vector3 _velocity;
        private bool _enabledMotor = true;

        public bool IsGrounded { get; private set; }
        public bool MotorEnabled => _enabledMotor;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (!_enabledMotor) return;

            var swim = GetComponent<PlayerSwim>();
            if (swim != null && swim.IsSwimming) return;

            IsGrounded = Physics.CheckSphere(
                transform.TransformPoint(groundCheckOffset),
                groundCheckRadius,
                groundMask,
                QueryTriggerInteraction.Ignore);

            float h = GameInput.Horizontal;
            float v = GameInput.Vertical;
            Vector3 input = new Vector3(h, 0f, v).normalized;

            Vector3 move = Vector3.zero;
            if (input.sqrMagnitude > 0.01f)
            {
                Vector3 camForward = cameraTransform != null ? cameraTransform.forward : transform.forward;
                Vector3 camRight = cameraTransform != null ? cameraTransform.right : transform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                move = (camForward * input.z + camRight * input.x).normalized;
                float speed = GameInput.SprintHeld ? runSpeed : walkSpeed;
                move *= speed;

                Quaternion targetRot = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));
            }

            if (IsGrounded && _velocity.y < 0f)
                _velocity.y = groundedGravity;

            if (IsGrounded && GameInput.JumpPressed)
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move((move + Vector3.up * _velocity.y) * Time.deltaTime);
        }

        public void SetMotorEnabled(bool enabled)
        {
            _enabledMotor = enabled;
            if (!enabled)
                _velocity = Vector3.zero;
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {
            bool wasEnabled = _controller.enabled;
            _controller.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            _controller.enabled = wasEnabled;
            _velocity = Vector3.zero;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
        }
#endif
    }
}
