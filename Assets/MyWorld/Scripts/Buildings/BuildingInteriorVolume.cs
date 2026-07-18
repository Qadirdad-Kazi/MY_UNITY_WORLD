using UnityEngine;

namespace MyWorld.Buildings
{
    /// <summary>
    /// Trigger volume for interiors (hide exterior audio, force camera clip, etc.).
    /// Add a BoxCollider with Is Trigger = true.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BuildingInteriorVolume : MonoBehaviour
    {
        [SerializeField] private string interiorName = "House Interior";
        [SerializeField] private bool disableSunShadowInside;
        [SerializeField] private Light directionalLight;

        public bool PlayerInside { get; private set; }

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerInside = true;
            if (disableSunShadowInside && directionalLight != null)
                directionalLight.shadows = LightShadows.None;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerInside = false;
            if (disableSunShadowInside && directionalLight != null)
                directionalLight.shadows = LightShadows.Soft;
        }

        public string InteriorName => interiorName;
    }
}
