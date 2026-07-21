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
    /// Seat anchor for the player. Place this empty where the driver should sit.
    /// For a pro look: Hide Player = OFF, tune Seat Offset / Euler until the character looks seated.
    /// </summary>
    public class VehicleSeat : MonoBehaviour
    {
        [SerializeField] private VehicleKind kind = VehicleKind.Car;
        [SerializeField] private Transform exitPoint;
        [Tooltip("Empty in the driver seat / saddle. If empty, this object is used.")]
        [SerializeField] private Transform seatPoint;
        [Tooltip("ON if chase cam shows the front of the bike/car (mesh faces opposite blue Z).")]
        [SerializeField] private bool invertChaseCamera;

        [Header("Player visibility")]
        [Tooltip("OFF = character stays visible and sits in the seat (recommended). ON = old hide style.")]
        [SerializeField] private bool hidePlayerWhileSeated;

        [Header("Seat pose (local to Seat Point)")]
        [Tooltip("Move the player so hips/hands look correct in the seat.")]
        [SerializeField] private Vector3 seatLocalOffset = new Vector3(0f, -0.4f, 0.08f);
        [Tooltip("Tilt the player. Car ≈ (10,0,0) · Bike ≈ (15,0,0).")]
        [SerializeField] private Vector3 seatLocalEuler = new Vector3(10f, 0f, 0f);
        [Tooltip("Extra size fix if Seat Point is a scaled mesh. 1 = normal. Try 0.01–0.1 if player is huge.")]
        [SerializeField] private float seatedScaleMultiplier = 1f;
        [Tooltip("Player root is at the FEET. Auto-drop so HIPS land on the Seat (fixes standing on the bike).")]
        [SerializeField] private bool autoDropHipsToSeat = true;

        [Header("Optional sit animation")]
        [Tooltip("If set, plays this Animator state name while seated (add clip in Animator first).")]
        [SerializeField] private string sitStateName = "";
        [SerializeField] private float sitCrossFade = 0.15f;

        public VehicleKind Kind => kind;
        public Transform ExitPoint => exitPoint != null ? exitPoint : transform;
        public Transform SitTransform => seatPoint != null ? seatPoint : transform;
        public bool InvertChaseCamera => invertChaseCamera;
        public bool HidePlayerWhileSeated => hidePlayerWhileSeated;
        public Vector3 SeatLocalOffset => seatLocalOffset;
        public Quaternion SeatLocalRotation => Quaternion.Euler(seatLocalEuler);
        public float SeatedScaleMultiplier => seatedScaleMultiplier;
        public bool AutoDropHipsToSeat => autoDropHipsToSeat;
        public string SitStateName => sitStateName;
        public float SitCrossFade => sitCrossFade;

        [ContextMenu("Apply Kind Defaults (Car/Bike/Boat)")]
        public void ApplyKindDefaults()
        {
            switch (kind)
            {
                case VehicleKind.Bike:
                    seatLocalOffset = new Vector3(0f, 0.05f, 0.05f);
                    seatLocalEuler = new Vector3(18f, 0f, 0f);
                    hidePlayerWhileSeated = false;
                    autoDropHipsToSeat = true;
                    break;
                case VehicleKind.Boat:
                    seatLocalOffset = new Vector3(0f, 0f, 0f);
                    seatLocalEuler = new Vector3(5f, 0f, 0f);
                    hidePlayerWhileSeated = false;
                    autoDropHipsToSeat = true;
                    break;
                default:
                    seatLocalOffset = new Vector3(0f, 0.05f, 0.08f);
                    seatLocalEuler = new Vector3(12f, 0f, 0f);
                    hidePlayerWhileSeated = false;
                    autoDropHipsToSeat = true;
                    break;
            }
        }

        private void OnValidate()
        {
            // keep serialized fields as authored
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var t = SitTransform;
            Gizmos.color = Color.cyan;
            Gizmos.matrix = t.localToWorldMatrix;
            Gizmos.DrawWireSphere(seatLocalOffset, 0.12f);
            Gizmos.DrawLine(seatLocalOffset, seatLocalOffset + SeatLocalRotation * Vector3.forward * 0.4f);
        }
#endif
    }
}
