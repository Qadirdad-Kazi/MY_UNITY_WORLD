using UnityEngine;

namespace MyWorld.Vehicles
{
    public enum VehicleKind
    {
        Car,
        Bike,
        Plane,
        Boat
    }

    /// <summary>
    /// Marks a seat transform. Parent should have VehicleEnterExit + a controller.
    /// </summary>
    public class VehicleSeat : MonoBehaviour
    {
        [SerializeField] private VehicleKind kind = VehicleKind.Car;
        [SerializeField] private Transform exitPoint;
        [SerializeField] private bool hidePlayerWhileSeated = true;

        public VehicleKind Kind => kind;
        public Transform ExitPoint => exitPoint != null ? exitPoint : transform;
        public bool HidePlayerWhileSeated => hidePlayerWhileSeated;
    }
}
