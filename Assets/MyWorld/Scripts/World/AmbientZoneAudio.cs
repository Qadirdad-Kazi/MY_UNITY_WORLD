using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// 3D ambient audio zone. Add AudioSource (Loop on) + trigger collider.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class AmbientZoneAudio : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed = 1.5f;
        [SerializeField] private float targetVolume = 0.55f;

        private AudioSource _source;
        private int _playersInside;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
            var src = GetComponent<AudioSource>();
            src.loop = true;
            src.playOnAwake = true;
            src.spatialBlend = 0f; // ambient bed; use 3D for local water/fire instead
            src.volume = 0f;
        }

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            float want = _playersInside > 0 ? targetVolume : 0f;
            _source.volume = Mathf.MoveTowards(_source.volume, want, fadeSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) _playersInside++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) _playersInside = Mathf.Max(0, _playersInside - 1);
        }
    }
}
