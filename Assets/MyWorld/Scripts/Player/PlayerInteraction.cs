using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Interaction;
using MyWorld.Vehicles;
using MyWorld.Core;

namespace MyWorld.Player
{
    /// <summary>
    /// Detects IInteractable near the player. Default key: E to enter.
    /// Exit is hold-E via VehicleExitBridge while driving.
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float range = 4f;
        [SerializeField] private float radius = 0.45f;
        [SerializeField] private LayerMask interactMask = ~0;
        [SerializeField] private Transform rayOrigin;

        private IInteractable _current;
        private bool _enabledInteraction = true;
        private VehicleEnterExit _driving;

        public string CurrentPrompt
        {
            get
            {
                if (_driving != null)
                    return _driving.CanExitNow ? "Hold E to exit" : string.Empty;
                return _current != null && _current.CanInteract(gameObject) ? _current.Prompt : string.Empty;
            }
        }

        private void Awake()
        {
            if (rayOrigin == null) rayOrigin = transform;
        }

        private void Update()
        {
            // Exit is handled by VehicleExitBridge (hold E) — do not tap-exit here
            if (_driving != null) return;

            if (!_enabledInteraction) return;

            _current = FindInteractable();
            bool pressed = GameInput.KeyDown(Key.E)
                || (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame);
            if (_current != null && pressed && _current.CanInteract(gameObject))
                _current.Interact(gameObject);
        }

        public void SetDrivingVehicle(VehicleEnterExit vehicle) => _driving = vehicle;

        private IInteractable FindInteractable()
        {
            Vector3 origin = rayOrigin.position + Vector3.up * 1.0f;
            Vector3 forward = rayOrigin.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.01f) forward = rayOrigin.forward;
            else forward.Normalize();

            IInteractable best = null;
            float bestScore = float.MaxValue;

            if (Physics.SphereCast(origin, radius, forward, out RaycastHit hit, range, interactMask, QueryTriggerInteraction.Collide))
            {
                var aimed = hit.collider.GetComponentInParent<IInteractable>();
                if (aimed != null && aimed.CanInteract(gameObject))
                    return aimed;
            }

            Collider[] cols = Physics.OverlapSphere(origin, range, interactMask, QueryTriggerInteraction.Collide);
            foreach (var c in cols)
            {
                if (c == null) continue;
                var i = c.GetComponentInParent<IInteractable>();
                if (i == null || !i.CanInteract(gameObject)) continue;

                Vector3 to = c.bounds.center - origin;
                float dist = to.magnitude;
                float facing = dist > 0.01f ? Vector3.Dot(forward, to.normalized) : 1f;
                float score = dist - Mathf.Max(0f, facing) * 2f;
                if (score < bestScore)
                {
                    bestScore = score;
                    best = i;
                }
            }

            return best;
        }

        public void SetInteractionEnabled(bool enabled) => _enabledInteraction = enabled;

        private void OnGUI()
        {
            // Only show enter prompt on foot — exit prompt is on VehicleExitBridge
            if (_driving != null) return;
            string prompt = CurrentPrompt;
            if (string.IsNullOrEmpty(prompt)) return;
            var rect = new Rect(Screen.width * 0.5f - 100f, Screen.height * 0.75f, 200f, 28f);
            GUI.Box(rect, prompt);
        }
    }
}
