using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Toggle headlights while driving (default key: L).
    /// Assign Spot Lights on the car/bike, or use context menu to create defaults.
    /// </summary>
    [RequireComponent(typeof(VehicleControllerBase))]
    public class VehicleLights : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private Light[] headlights;
        [SerializeField] private Light[] extraLights;
        [SerializeField] private bool startOn;

        [Header("Key (while driving)")]
        [SerializeField] private Key toggleKey = Key.L;

        private VehicleControllerBase _controller;
        private bool _on;

        private void Awake()
        {
            _controller = GetComponent<VehicleControllerBase>();
            _on = startOn;
            Apply();
        }

        private void Update()
        {
            if (_controller == null || !_controller.IsPlayerDriving) return;
            if (!GameInput.KeyDown(toggleKey)) return;

            _on = !_on;
            Apply();
        }

        private void Apply()
        {
            SetGroup(headlights, _on);
            SetGroup(extraLights, _on);
        }

        private static void SetGroup(Light[] lights, bool on)
        {
            if (lights == null) return;
            foreach (var l in lights)
            {
                if (l != null) l.enabled = on;
            }
        }

        public void SetLights(bool on)
        {
            _on = on;
            Apply();
        }

        public bool AreOn => _on;

#if UNITY_EDITOR
        [ContextMenu("Create Default Headlight Spots")]
        private void CreateDefaultHeadlightSpots()
        {
            var list = new System.Collections.Generic.List<Light>();
            list.Add(MakeSpot("Headlight_L", new Vector3(-0.35f, 0.7f, 1.1f)));
            list.Add(MakeSpot("Headlight_R", new Vector3(0.35f, 0.7f, 1.1f)));
            headlights = list.ToArray();
            _on = startOn;
            Apply();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private Light MakeSpot(string name, Vector3 localPos)
        {
            Transform existing = transform.Find(name);
            GameObject go = existing != null ? existing.gameObject : new GameObject(name);
            if (existing == null) go.transform.SetParent(transform, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.identity;

            var light = go.GetComponent<Light>();
            if (light == null) light = go.AddComponent<Light>();
            light.type = LightType.Spot;
            light.range = 35f;
            light.spotAngle = 55f;
            light.intensity = 6f;
            light.color = new Color(1f, 0.95f, 0.85f);
            light.shadows = LightShadows.Soft;
            light.enabled = startOn;
            return light;
        }
#endif
    }
}
