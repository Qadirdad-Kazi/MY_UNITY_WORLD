using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Fires when player enters a zone. Use for UI name popups / audio swaps later.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ZoneTrigger : MonoBehaviour
    {
        [SerializeField] private string zoneId = "Zone_Village";
        [SerializeField] private string displayName = "Village";
        [SerializeField] private bool debugLog = true;

        public string ZoneId => zoneId;
        public string DisplayName => displayName;
        public static string CurrentZoneId { get; private set; }
        public static string CurrentZoneName { get; private set; }

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            CurrentZoneId = zoneId;
            CurrentZoneName = displayName;
            if (debugLog) Debug.Log($"Entered zone: {displayName}", this);
        }
    }
}
