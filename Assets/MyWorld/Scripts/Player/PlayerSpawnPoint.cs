using UnityEngine;

namespace MyWorld.Player
{
    /// <summary>
    /// Empty marker. Place in scene, tag Spawn. PlayerBootstrap uses it.
    /// </summary>
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.4f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
        }
    }
}
