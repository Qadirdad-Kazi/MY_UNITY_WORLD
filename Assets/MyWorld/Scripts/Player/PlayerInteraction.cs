using UnityEngine;
using MyWorld.Interaction;

namespace MyWorld.Player
{
    /// <summary>
    /// Detects IInteractable in front of the player. Default key: E.
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float range = 2.5f;
        [SerializeField] private float radius = 0.35f;
        [SerializeField] private LayerMask interactMask = ~0;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private Transform rayOrigin;

        private IInteractable _current;
        private bool _enabledInteraction = true;

        public string CurrentPrompt => _current != null && _current.CanInteract(gameObject) ? _current.Prompt : string.Empty;

        private void Awake()
        {
            if (rayOrigin == null) rayOrigin = transform;
        }

        private void Update()
        {
            if (!_enabledInteraction) return;

            _current = FindInteractable();
            if (_current != null && Input.GetKeyDown(interactKey) && _current.CanInteract(gameObject))
                _current.Interact(gameObject);
        }

        private IInteractable FindInteractable()
        {
            Vector3 origin = rayOrigin.position + Vector3.up * 1.2f;
            Vector3 dir = rayOrigin.forward;

            if (Physics.SphereCast(origin, radius, dir, out RaycastHit hit, range, interactMask, QueryTriggerInteraction.Collide))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null) return interactable;
            }

            // Fallback overlap at reach point
            Collider[] cols = Physics.OverlapSphere(origin + dir * range * 0.6f, radius * 1.5f, interactMask, QueryTriggerInteraction.Collide);
            float best = float.MaxValue;
            IInteractable bestInteractable = null;
            foreach (var c in cols)
            {
                var i = c.GetComponentInParent<IInteractable>();
                if (i == null) continue;
                float d = (c.transform.position - origin).sqrMagnitude;
                if (d < best)
                {
                    best = d;
                    bestInteractable = i;
                }
            }
            return bestInteractable;
        }

        public void SetInteractionEnabled(bool enabled) => _enabledInteraction = enabled;

        private void OnGUI()
        {
            if (!_enabledInteraction) return;
            string prompt = CurrentPrompt;
            if (string.IsNullOrEmpty(prompt)) return;
            // Temporary debug prompt — replace with UI Toolkit / uGUI in production.
            var rect = new Rect(Screen.width * 0.5f - 120f, Screen.height * 0.72f, 240f, 28f);
            GUI.Box(rect, prompt);
        }
    }
}
