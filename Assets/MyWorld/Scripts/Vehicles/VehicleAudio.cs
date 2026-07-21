using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Engine loop (pitch by speed), horn, optional radio. Works while player is driving.
    /// Add to same root as Car/Bike Controller. Assign AudioClips in Inspector.
    /// </summary>
    [RequireComponent(typeof(VehicleControllerBase))]
    public class VehicleAudio : MonoBehaviour
    {
        [Header("Clips (assign your own)")]
        [SerializeField] private AudioClip engineLoop;
        [SerializeField] private AudioClip hornClip;
        [SerializeField] private AudioClip radioMusic;

        [Header("Keys (while driving)")]
        [SerializeField] private Key hornKey = Key.F;
        [SerializeField] private Key radioKey = Key.M;

        [Header("Engine")]
        [SerializeField] private float idlePitch = 0.75f;
        [SerializeField] private float maxPitch = 1.65f;
        [SerializeField] private float maxSpeedKmh = 100f;
        [SerializeField] private float engineVolume = 0.55f;
        [SerializeField] private float hornVolume = 0.9f;
        [SerializeField] private float radioVolume = 0.25f;

        private VehicleControllerBase _controller;
        private Rigidbody _rb;
        private AudioSource _engine;
        private AudioSource _horn;
        private AudioSource _radio;
        private bool _radioOn;

        private void Awake()
        {
            _controller = GetComponent<VehicleControllerBase>();
            _rb = GetComponent<Rigidbody>();
            _engine = CreateSource("Audio_Engine", true);
            _horn = CreateSource("Audio_Horn", false);
            _radio = CreateSource("Audio_Radio", true);
            StopAll();
        }

        private AudioSource CreateSource(string childName, bool loop)
        {
            var t = transform.Find(childName);
            GameObject go = t != null ? t.gameObject : new GameObject(childName);
            if (t == null) go.transform.SetParent(transform, false);

            var src = go.GetComponent<AudioSource>();
            if (src == null) src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = loop;
            src.spatialBlend = 1f;
            src.dopplerLevel = 0.15f;
            src.minDistance = 4f;
            src.maxDistance = 45f;
            src.rolloffMode = AudioRolloffMode.Linear;
            return src;
        }

        private void Update()
        {
            bool driving = _controller != null && _controller.IsPlayerDriving;
            if (!driving)
            {
                if (_engine.isPlaying) _engine.Stop();
                if (_radio.isPlaying) _radio.Stop();
                _radioOn = false;
                return;
            }

            UpdateEngine();
            HandleHorn();
            HandleRadio();
        }

        private void UpdateEngine()
        {
            if (engineLoop == null) return;

            if (_engine.clip != engineLoop) _engine.clip = engineLoop;
            if (!_engine.isPlaying) _engine.Play();

            float speed = _rb != null ? _rb.linearVelocity.magnitude * 3.6f : 0f;
            float throttle = Mathf.Abs(GameInput.Vertical);
            float t = Mathf.Clamp01(Mathf.Max(speed / Mathf.Max(1f, maxSpeedKmh), throttle * 0.65f));
            _engine.pitch = Mathf.Lerp(idlePitch, maxPitch, t);
            _engine.volume = engineVolume * (0.55f + 0.45f * t);
        }

        private void HandleHorn()
        {
            if (hornClip == null) return;
            if (!GameInput.KeyDown(hornKey)) return;

            _horn.PlayOneShot(hornClip, hornVolume);
        }

        private void HandleRadio()
        {
            if (radioMusic == null) return;
            if (!GameInput.KeyDown(radioKey)) return;

            _radioOn = !_radioOn;
            if (_radioOn)
            {
                _radio.clip = radioMusic;
                _radio.volume = radioVolume;
                _radio.Play();
            }
            else
            {
                _radio.Stop();
            }
        }

        private void StopAll()
        {
            if (_engine != null) _engine.Stop();
            if (_horn != null) _horn.Stop();
            if (_radio != null) _radio.Stop();
        }

        private void OnDisable() => StopAll();
    }
}
