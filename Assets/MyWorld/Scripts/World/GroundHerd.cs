using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Spawns several GroundAnimal copies (herd / pack) on the terrain surface.
    /// </summary>
    public class GroundHerd : MonoBehaviour
    {
        [SerializeField] private GameObject animalPrefab;
        [SerializeField] private int count = 5;
        [SerializeField] private Vector2 spawnRadius = new Vector2(8f, 8f);
        [SerializeField] private bool addGroundAnimalIfMissing = true;
        [SerializeField] private float walkSpeedMin = 1.4f;
        [SerializeField] private float walkSpeedMax = 2.2f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float wanderRadius = 16f;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private float probeHeight = 8f;

        private void Start()
        {
            if (animalPrefab == null)
            {
                Debug.LogWarning("[GroundHerd] Assign an Animal Prefab (deer, rabbit, dog, etc.).", this);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 local = new Vector3(
                    Random.Range(-spawnRadius.x, spawnRadius.x),
                    0f,
                    Random.Range(-spawnRadius.y, spawnRadius.y));
                Vector3 pos = transform.position + local;
                if (Physics.Raycast(pos + Vector3.up * probeHeight, Vector3.down, out RaycastHit hit, probeHeight * 2f, groundMask, QueryTriggerInteraction.Ignore))
                    pos = hit.point;

                var go = Instantiate(animalPrefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), transform);
                go.name = $"{animalPrefab.name}_Ground_{i + 1}";

                var animal = go.GetComponent<GroundAnimal>();
                if (animal == null && addGroundAnimalIfMissing)
                    animal = go.AddComponent<GroundAnimal>();

                if (animal != null)
                    animal.ConfigureRuntime(Random.Range(walkSpeedMin, walkSpeedMax), runSpeed, wanderRadius);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.45f, 0.9f, 0.4f, 0.3f);
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius.x * 2f, 0.2f, spawnRadius.y * 2f));
        }
#endif
    }
}
