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
    /// Add to the same object as DayNightCycle.
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

        [Header("Rain")]
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private Transform rainFollowTarget;
        [SerializeField] private float rainHeight = 25f;

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
            if (dayNight == null) dayNight = FindFirstObjectByType<DayNightCycle>();
            EnsureRainParticles();
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
                // ZoneFog turns fog on/off in LateUpdate — do not force global fog here
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
                if (rainParticles.isPlaying) rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                return;
            }

            emission.rateOverTime = _current == WeatherType.Storm ? 1800f : 800f;
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
            rainParticles.transform.position = follow.position + Vector3.up * rainHeight;
        }

        private void EnsureRainParticles()
        {
            if (rainParticles != null) return;

            var go = new GameObject("RainParticles");
            go.transform.SetParent(transform);
            rainParticles = go.AddComponent<ParticleSystem>();

            var main = rainParticles.main;
            main.loop = true;
            main.startLifetime = 2.5f;
            main.startSpeed = 18f;
            main.startSize = 0.08f;
            main.maxParticles = 8000;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startColor = new Color(0.75f, 0.82f, 0.95f, 0.55f);

            var shape = rainParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(40f, 1f, 40f);

            var renderer = rainParticles.GetComponent<ParticleSystemRenderer>();
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null) shader = Shader.Find("Particles/Standard Unlit");
            if (shader != null)
                renderer.material = new Material(shader);

            rainParticles.Stop();
        }
    }
}
