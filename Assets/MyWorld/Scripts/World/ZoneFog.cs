using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Fog ONLY inside this trigger zone (forest, etc.).
    /// Weather System global fog should be OFF — use this for local mist.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ZoneFog : MonoBehaviour
    {
        [SerializeField] private float fogDensity = 0.025f;
        [SerializeField] private Color fogColor = new Color(0.72f, 0.78f, 0.72f);

        private static bool _insideAnyZone;
        private static float _density;
        private static Color _color;

        public static bool IsInsideZone => _insideAnyZone;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _insideAnyZone = true;
            _density = fogDensity;
            _color = fogColor;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _insideAnyZone = false;
        }

        private void LateUpdate()
        {
            if (_insideAnyZone)
            {
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.ExponentialSquared;
                RenderSettings.fogDensity = _density;
                RenderSettings.fogColor = _color;
            }
            else
            {
                RenderSettings.fog = false;
            }
        }
    }
}
