using UnityEngine;

namespace MyWorld.Player
{
    /// <summary>
    /// Optional: moves player to PlayerSpawnPoint on play.
    /// </summary>
    public class PlayerBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerMotor player;
        [SerializeField] private bool snapOnStart = true;

        private void Start()
        {
            if (!snapOnStart) return;
            if (player == null) player = FindFirstObjectByType<PlayerMotor>();
            var spawn = FindFirstObjectByType<PlayerSpawnPoint>();
            if (player == null || spawn == null) return;
            player.Teleport(spawn.transform.position, spawn.transform.rotation);
        }
    }
}
