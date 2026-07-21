using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Trigger volume for swimming. Put a Box Collider (Is Trigger) covering the water.
    /// Surface Y = water plane height (boats / swim use the same number).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WaterVolume : MonoBehaviour
    {
        [Tooltip("Water surface height. If Use Transform Y is on, uses this object's Y instead.")]
        [SerializeField] private float surfaceY = 10f;
        [SerializeField] private bool useTransformY = true;

        public float SurfaceY => useTransformY ? transform.position.y : surfaceY;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) => Notify(other, true);

        private void OnTriggerStay(Collider other)
        {
            // Re-assert swim every stay frame so gravity can't "win" if enter was missed
            var swim = other.GetComponentInParent<MyWorld.Player.PlayerSwim>();
            if (swim != null) swim.EnterWater(this);
        }

        private void OnTriggerExit(Collider other) => Notify(other, false);

        private void Notify(Collider other, bool entered)
        {
            var swim = other.GetComponentInParent<MyWorld.Player.PlayerSwim>();
            if (swim == null) return;
            if (entered) swim.EnterWater(this);
            else swim.ExitWater(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var col = GetComponent<Collider>();
            if (col == null) return;
            Gizmos.color = new Color(0.2f, 0.55f, 1f, 0.25f);
            Gizmos.matrix = transform.localToWorldMatrix;
            if (col is BoxCollider box)
                Gizmos.DrawCube(box.center, box.size);
            else if (col is SphereCollider sphere)
                Gizmos.DrawSphere(sphere.center, sphere.radius);
        }

        private void OnDrawGizmosSelected()
        {
            float y = SurfaceY;
            Gizmos.color = Color.cyan;
            Vector3 p = transform.position;
            Gizmos.DrawLine(new Vector3(p.x - 20f, y, p.z - 20f), new Vector3(p.x + 20f, y, p.z + 20f));
            Gizmos.DrawLine(new Vector3(p.x - 20f, y, p.z + 20f), new Vector3(p.x + 20f, y, p.z - 20f));
        }
#endif
    }
}
