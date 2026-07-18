using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Rotates directional light for a simple day/night cycle.
    /// Attach to an empty "DayNight" object and assign the sun.
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private Light sun;
        [SerializeField] private float dayLengthMinutes = 12f;
        [SerializeField] private AnimationCurve intensityByTime = AnimationCurve.EaseInOut(0f, 0.05f, 0.5f, 1.2f);
        [SerializeField] private Gradient sunColor;
        [SerializeField] private bool previewInEditMode;

        [Range(0f, 1f)]
        [SerializeField] private float timeOfDay = 0.3f;

        private void Reset()
        {
            sun = RenderSettings.sun != null ? RenderSettings.sun : FindFirstObjectByType<Light>();
            sunColor = new Gradient();
            sunColor.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(1f, 0.6f, 0.3f), 0f),
                    new GradientColorKey(Color.white, 0.35f),
                    new GradientColorKey(new Color(1f, 0.75f, 0.5f), 0.7f),
                    new GradientColorKey(new Color(0.2f, 0.25f, 0.45f), 1f)
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
            if (sun == null) return;
            float sunAngle = timeOfDay * 360f - 90f;
            sun.transform.rotation = Quaternion.Euler(sunAngle, 30f, 0f);
            sun.intensity = intensityByTime.Evaluate(timeOfDay);
            if (sunColor != null) sun.color = sunColor.Evaluate(timeOfDay);
        }

        public void SetTimeOfDay(float t01) => timeOfDay = Mathf.Clamp01(t01);
    }
}
