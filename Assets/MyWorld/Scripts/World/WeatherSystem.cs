using UnityEngine;

namespace MyWorld.World
{
    public enum WeatherType
    {
        Clear,
        Cloudy,
        Rain,
        Storm
    }

    /// <summary>
    /// Random weather: swaps AllSkyFree skies, fog, rain particles.
    /// Rain follows the camera and covers the visible play area.
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        [Header("Time")]
        [SerializeField] private DayNightCycle dayNight;
        [Tooltip("Night sky with stars.")]
        [SerializeField] private Material nightSky;

        [Header("Day skies (drag from Assets/AllSkyFree/)")]
        [SerializeField] private Material clearSky;
        [SerializeField] private Material cloudySky;
        [SerializeField] private Material rainSky;
        [SerializeField] private Material stormSky;

        [Header("Random change")]
        [SerializeField] private float minWeatherMinutes = 4f;
        [SerializeField] private float maxWeatherMinutes = 10f;
        [SerializeField] private bool randomizeOnStart = true;

        [Header("Rain (covers camera view)")]
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private Transform rainFollowTarget;
        [Tooltip("How high above the camera the rain box sits.")]
        [SerializeField] private float rainHeight = 18f;
        [Tooltip("Width/depth of rain around the camera (visible area).")]
        [SerializeField] private float rainCoverage = 90f;
        [SerializeField] private float rainRate = 2500f;
        [SerializeField] private float stormRate = 5500f;

        [Header("Sun dim (day only)")]
        [SerializeField] private float clearSunMultiplier = 1f;
        [SerializeField] private float cloudySunMultiplier = 0.65f;
        [SerializeField] private float rainSunMultiplier = 0.45f;
        [SerializeField] private float stormSunMultiplier = 0.3f;

        [Header("Fog (whole world)")]
        [Tooltip("OFF = fog only where ZoneFog trigger boxes are (forest). ON = fog everywhere when raining.")]
        [SerializeField] private bool enableGlobalFog = false;

        private WeatherType _current = WeatherType.Clear;
        private Material _activeSky;
        private float _nextChangeTime;

        public WeatherType CurrentWeather => _current;

        public float GetSunMultiplier() => _current switch
        {
            WeatherType.Cloudy => cloudySunMultiplier,
            WeatherType.Rain => rainSunMultiplier,
            WeatherType.Storm => stormSunMultiplier,
            _ => clearSunMultiplier
        };

        private void Awake()
        {
            if (dayNight == null) dayNight = GetComponent<DayNightCycle>();
            if (dayNight == null) dayNight = FindAnyObjectByType<DayNightCycle>();
            EnsureRainParticles();
            ConfigureRainParticles();
        }

        private void Start()
        {
            ScheduleNextChange(30f);
            if (randomizeOnStart)
                SetWeather(PickRandomWeather());
            else
                ApplySkyAndFog();
        }

        private void Update()
        {
            if (Time.time >= _nextChangeTime)
            {
                SetWeather(PickRandomWeather());
                ScheduleNextChange();
            }

            FollowRain();
            ApplySkyAndFog();
            ApplyRain();
        }

        public void SetWeather(WeatherType type)
        {
            _current = type;
            ApplySkyAndFog();
            ApplyRain();
        }

        private void ApplySkyAndFog()
        {
            bool isNight = dayNight != null && dayNight.IsNightTime;

            Material target = isNight ? nightSky : GetDaySky(_current);
            if (target != null && target != _activeSky)
            {
                RenderSettings.skybox = target;
                _activeSky = target;
            }

            if (!enableGlobalFog)
            {
                if (!ZoneFog.IsInsideZone)
                    RenderSettings.fog = false;
                return;
            }

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = isNight ? 0.008f : _current switch
            {
                WeatherType.Cloudy => 0.004f,
                WeatherType.Rain => 0.012f,
                WeatherType.Storm => 0.022f,
                _ => 0.002f
            };
        }

        private Material GetDaySky(WeatherType type) => type switch
        {
            WeatherType.Cloudy => cloudySky != null ? cloudySky : clearSky,
            WeatherType.Rain => rainSky != null ? rainSky : cloudySky ?? clearSky,
            WeatherType.Storm => stormSky != null ? stormSky : rainSky ?? cloudySky ?? clearSky,
            _ => clearSky
        };

        private void ApplyRain()
        {
            if (rainParticles == null) return;
            bool isNight = dayNight != null && dayNight.IsNightTime;
            bool shouldRain = !isNight && (_current == WeatherType.Rain || _current == WeatherType.Storm);

            var emission = rainParticles.emission;
            if (!shouldRain)
            {
                emission.rateOverTime = 0f;
                if (rainParticles.isPlaying)
                    rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                return;
            }

            emission.rateOverTime = _current == WeatherType.Storm ? stormRate : rainRate;
            if (!rainParticles.isPlaying) rainParticles.Play();
        }

        private WeatherType PickRandomWeather()
        {
            float r = Random.value;
            if (r < 0.35f) return WeatherType.Clear;
            if (r < 0.60f) return WeatherType.Cloudy;
            if (r < 0.85f) return WeatherType.Rain;
            return WeatherType.Storm;
        }

        private void ScheduleNextChange(float delaySeconds = -1f)
        {
            if (delaySeconds < 0f)
                delaySeconds = Random.Range(minWeatherMinutes, maxWeatherMinutes) * 60f;
            _nextChangeTime = Time.time + delaySeconds;
        }

        private void FollowRain()
        {
            if (rainParticles == null) return;
            Transform follow = rainFollowTarget;
            if (follow == null && Camera.main != null) follow = Camera.main.transform;
            if (follow == null) return;

            // Keep rain centered on what the player sees (camera), slightly ahead so forward view is covered
            Vector3 ahead = follow.forward;
            ahead.y = 0f;
            if (ahead.sqrMagnitude > 0.01f) ahead.Normalize();
            else ahead = Vector3.zero;

            rainParticles.transform.position = follow.position
                + Vector3.up * rainHeight
                + ahead * (rainCoverage * 0.15f);
            rainParticles.transform.rotation = Quaternion.identity;
        }

        private void EnsureRainParticles()
        {
            if (rainParticles != null) return;

            var go = new GameObject("RainParticles");
            go.transform.SetParent(transform);
            rainParticles = go.AddComponent<ParticleSystem>();
            rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        /// <summary>
        /// Wide, fast streaks over the visible area — not a tiny cloud over the player.
        /// </summary>
        private void ConfigureRainParticles()
        {
            if (rainParticles == null) return;

            var main = rainParticles.main;
            main.loop = true;
            main.playOnAwake = false;
            main.startLifetime = 1.1f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.9f, 1.35f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(22f, 32f);
            main.startSize3D = true;
            main.startSizeX = new ParticleSystem.MinMaxCurve(0.015f, 0.03f);
            main.startSizeY = new ParticleSystem.MinMaxCurve(0.35f, 0.7f); // streak length
            main.startSizeZ = new ParticleSystem.MinMaxCurve(0.015f, 0.03f);
            main.startRotation3D = true;
            main.maxParticles = 20000;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = 0.85f;
            main.startColor = new Color(0.78f, 0.85f, 0.95f, 0.45f);

            var emission = rainParticles.emission;
            emission.rateOverTime = 0f;

            var shape = rainParticles.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(rainCoverage, 2f, rainCoverage);
            shape.position = Vector3.zero;

            var vel = rainParticles.velocityOverLifetime;
            vel.enabled = true;
            vel.space = ParticleSystemSimulationSpace.Local;
            // Slight forward slant so streaks look less “fake vertical dots”
            vel.x = new ParticleSystem.MinMaxCurve(-1.5f, 1.5f);
            vel.y = new ParticleSystem.MinMaxCurve(-8f, -2f);
            vel.z = new ParticleSystem.MinMaxCurve(-1.5f, 1.5f);

            var noise = rainParticles.noise;
            noise.enabled = true;
            noise.strength = 0.15f;
            noise.frequency = 0.2f;
            noise.scrollSpeed = 0.3f;
            noise.octaveCount = 1;

            var renderer = rainParticles.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Stretch;
            renderer.lengthScale = 2.2f;
            renderer.velocityScale = 0.12f;
            renderer.cameraVelocityScale = 0f;
            renderer.alignment = ParticleSystemRenderSpace.View;

            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null) shader = Shader.Find("Particles/Standard Unlit");
            if (shader != null)
            {
                var mat = new Material(shader);
                mat.SetFloat("_Surface", 1f); // transparent if available
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", new Color(0.8f, 0.88f, 1f, 0.55f));
                renderer.material = mat;
                renderer.trailMaterial = mat;
            }

            rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            rainCoverage = Mathf.Clamp(rainCoverage, 40f, 250f);
            rainHeight = Mathf.Clamp(rainHeight, 8f, 60f);
        }
#endif
    }
}
