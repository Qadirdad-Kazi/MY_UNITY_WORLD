using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Day/night: moves the sun light and optionally swaps AllSkyFree skyboxes
    /// (day sky has a baked sun — use a night sky material after dark).
    /// If WeatherSystem is present, it controls the sky instead.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private Light sun;
        [SerializeField] private float dayLengthMinutes = 12f;
        [SerializeField] private AnimationCurve intensityByTime = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1.2f);
        [SerializeField] private Gradient sunColor;
        [SerializeField] private bool previewInEditMode;

        [Header("Sky swap (fixes visible sun at night)")]
        [Tooltip("Day sky, e.g. Assets/AllSkyFree/Epic_BlueSunset/Epic_BlueSunset.mat")]
        [SerializeField] private Material daySky;
        [Tooltip("Night sky, e.g. Assets/AllSkyFree/Cold Night/Cold Night.mat")]
        [SerializeField] private Material nightSky;
        [Tooltip("Time 0–1 when night sky starts (after sunset).")]
        [SerializeField] private float nightStart = 0.72f;
        [Tooltip("Time 0–1 when day sky returns (after sunrise).")]
        [SerializeField] private float dayStart = 0.28f;

        [Range(0f, 1f)]
        [SerializeField] private float timeOfDay = 0.3f;

        private Material _activeSky;
        private WeatherSystem _weather;

        public float TimeOfDay => timeOfDay;
        public float NightStart => nightStart;
        public float DayStart => dayStart;
        public bool IsNightTime => timeOfDay >= nightStart || timeOfDay < dayStart;

        private void Awake()
        {
            _weather = GetComponent<WeatherSystem>();
            if (_weather == null) _weather = FindAnyObjectByType<WeatherSystem>();
        }

        private void Reset()
        {
            sun = RenderSettings.sun != null ? RenderSettings.sun : FindAnyObjectByType<Light>();
            sunColor = new Gradient();
            sunColor.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(0.15f, 0.2f, 0.35f), 0f),
                    new GradientColorKey(new Color(1f, 0.55f, 0.35f), 0.28f),
                    new GradientColorKey(Color.white, 0.5f),
                    new GradientColorKey(new Color(1f, 0.6f, 0.35f), 0.72f),
                    new GradientColorKey(new Color(0.15f, 0.2f, 0.35f), 1f)
                },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
        }

        private void Update()
        {
            if (!Application.isPlaying && !previewInEditMode) return;
            if (dayLengthMinutes > 0.01f && Application.isPlaying)
                timeOfDay = Mathf.Repeat(timeOfDay + Time.deltaTime / (dayLengthMinutes * 60f), 1f);

            Apply();
        }

        private void Apply()
        {
            if (sun != null)
            {
                float sunAngle = timeOfDay * 360f - 90f;
                sun.transform.rotation = Quaternion.Euler(sunAngle, 30f, 0f);
                float intensity = intensityByTime.Evaluate(timeOfDay);
                if (_weather != null && !IsNightTime)
                    intensity *= _weather.GetSunMultiplier();
                sun.intensity = intensity;
                if (sunColor != null) sun.color = sunColor.Evaluate(timeOfDay);
            }

            if (_weather == null)
                ApplySky();
        }

        private void ApplySky()
        {
            if (daySky == null && nightSky == null) return;

            bool isNight = IsNightTime;
            Material target = isNight ? nightSky : daySky;
            if (target == null) target = daySky != null ? daySky : nightSky;
            if (target == null || target == _activeSky) return;

            RenderSettings.skybox = target;
            _activeSky = target;
        }

        public void SetTimeOfDay(float t01)
        {
            timeOfDay = Mathf.Clamp01(t01);
            Apply();
        }
    }
}
