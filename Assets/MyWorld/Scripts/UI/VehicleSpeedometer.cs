using UnityEngine;
using UnityEngine.UI;
using MyWorld.Vehicles;

namespace MyWorld.UI
{
    /// <summary>
    /// Simple km/h speedometer while driving. No extra asset pack required —
    /// uses uGUI Text (and optional Image fill for a needle/bar).
    /// Put on a UI object, or let it create a basic overlay at runtime.
    /// </summary>
    public class VehicleSpeedometer : MonoBehaviour
    {
        [Header("UI (optional — auto-created if empty)")]
        [SerializeField] private Text speedText;
        [SerializeField] private Image speedFill;
        [SerializeField] private bool createUiIfMissing = true;
        [SerializeField] private string labelFormat = "{0:0} km/h";
        [SerializeField] private float maxSpeedForFill = 160f;

        [Header("Style")]
        [SerializeField] private Vector2 screenOffset = new Vector2(0f, 48f);
        [SerializeField] private int fontSize = 28;

        private VehicleControllerBase _driving;
        private Rigidbody _rb;
        private Canvas _canvas;
        private GameObject _root;
        private float _nextSearchTime;

        private void Awake()
        {
            if (speedText == null && createUiIfMissing)
                BuildDefaultUi();
            SetVisible(false);
        }

        private void Update()
        {
            if (_driving == null || !_driving.IsPlayerDriving || _rb == null)
            {
                if (Time.unscaledTime < _nextSearchTime)
                {
                    SetVisible(false);
                    return;
                }

                _nextSearchTime = Time.unscaledTime + 0.25f;
                if (!TryFindDrivingVehicle())
                {
                    SetVisible(false);
                    return;
                }
            }

            if (!_driving.IsPlayerDriving)
            {
                SetVisible(false);
                _driving = null;
                _rb = null;
                return;
            }

            float kmh = _rb.linearVelocity.magnitude * 3.6f;
            SetVisible(true);

            if (speedText != null)
                speedText.text = string.Format(labelFormat, kmh);

            if (speedFill != null && maxSpeedForFill > 0.1f)
                speedFill.fillAmount = Mathf.Clamp01(kmh / maxSpeedForFill);
        }

        private bool TryFindDrivingVehicle()
        {
            var all = FindObjectsByType<VehicleControllerBase>(FindObjectsSortMode.None);
            foreach (var v in all)
            {
                if (v != null && v.IsPlayerDriving)
                {
                    _driving = v;
                    _rb = v.GetComponent<Rigidbody>();
                    return _rb != null;
                }
            }
            return false;
        }

        private void SetVisible(bool on)
        {
            if (_root != null) _root.SetActive(on);
            else if (speedText != null) speedText.enabled = on;
        }

        private void BuildDefaultUi()
        {
            _canvas = FindAnyObjectByType<Canvas>();
            if (_canvas == null)
            {
                var canvasGo = new GameObject("SpeedometerCanvas");
                _canvas = canvasGo.AddComponent<Canvas>();
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasGo.AddComponent<GraphicRaycaster>();
            }

            _root = new GameObject("Speedometer");
            _root.transform.SetParent(_canvas.transform, false);

            var rt = _root.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = screenOffset;
            rt.sizeDelta = new Vector2(220f, 48f);

            // Soft background
            var bgGo = new GameObject("Bg");
            bgGo.transform.SetParent(_root.transform, false);
            var bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;
            var bg = bgGo.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.45f);

            // Optional fill bar
            var fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(_root.transform, false);
            var fillRt = fillGo.AddComponent<RectTransform>();
            fillRt.anchorMin = new Vector2(0.08f, 0.12f);
            fillRt.anchorMax = new Vector2(0.92f, 0.38f);
            fillRt.offsetMin = Vector2.zero;
            fillRt.offsetMax = Vector2.zero;
            speedFill = fillGo.AddComponent<Image>();
            speedFill.color = new Color(0.95f, 0.75f, 0.2f, 0.85f);
            speedFill.type = Image.Type.Filled;
            speedFill.fillMethod = Image.FillMethod.Horizontal;
            speedFill.fillAmount = 0f;

            // Speed text
            var textGo = new GameObject("SpeedText");
            textGo.transform.SetParent(_root.transform, false);
            var textRt = textGo.AddComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0f, 0.35f);
            textRt.anchorMax = new Vector2(1f, 1f);
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            speedText = textGo.AddComponent<Text>();
            speedText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (speedText.font == null)
                speedText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            speedText.fontSize = fontSize;
            speedText.alignment = TextAnchor.MiddleCenter;
            speedText.color = Color.white;
            speedText.text = "0 km/h";
        }
    }
}
