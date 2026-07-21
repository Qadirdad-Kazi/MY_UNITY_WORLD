using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Spawns several flying copies (bird flock / fly swarm).
    /// Assign a bird/eagle/fly prefab; FlyingAnimal is added automatically if missing.
    /// </summary>
    public class FlyingFlock : MonoBehaviour
    {
        [SerializeField] private GameObject animalPrefab;
        [SerializeField] private int count = 6;
        [SerializeField] private Vector3 spawnBox = new Vector3(20f, 6f, 20f);
        [SerializeField] private bool addFlyingAnimalIfMissing = true;
        [SerializeField] private FlyingAnimal.FlyStyle style = FlyingAnimal.FlyStyle.Wander;
        [SerializeField] private float speedMin = 5f;
        [SerializeField] private float speedMax = 8f;

        private void Start()
        {
            if (animalPrefab == null)
            {
                Debug.LogWarning("[FlyingFlock] Assign an Animal Prefab (bird / eagle / fly mesh).", this);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 local = new Vector3(
                    Random.Range(-spawnBox.x, spawnBox.x) * 0.5f,
                    Random.Range(-spawnBox.y, spawnBox.y) * 0.5f,
                    Random.Range(-spawnBox.z, spawnBox.z) * 0.5f);

                var go = Instantiate(
                    animalPrefab,
                    transform.position + local,
                    Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                    transform);
                go.name = $"{animalPrefab.name}_Fly_{i + 1}";

                var fly = go.GetComponent<FlyingAnimal>();
                if (fly == null && addFlyingAnimalIfMissing)
                    fly = go.AddComponent<FlyingAnimal>();

                if (fly != null)
                    fly.ConfigureRuntime(style, Random.Range(speedMin, speedMax));
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.85f, 1f, 0.25f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, spawnBox);
        }
#endif
    }
}
