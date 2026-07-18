using UnityEngine;
using MyWorld.Interaction;
using MyWorld.Player;

namespace MyWorld.Buildings
{
    /// <summary>
    /// Simple house door. Modes:
    /// - TeleportToInterior: moves player to interiorSpawn
    /// - ToggleDoorRotation: swings the door transform
    /// </summary>
    public class HouseDoor : InteractableBase
    {
        public enum DoorMode
        {
            TeleportToInterior,
            ToggleDoorRotation
        }

        [SerializeField] private DoorMode mode = DoorMode.TeleportToInterior;
        [SerializeField] private Transform interiorSpawn;
        [SerializeField] private Transform exteriorSpawn;
        [SerializeField] private Transform doorPivot;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float swingSpeed = 6f;
        [SerializeField] private bool isOpen;
        [SerializeField] private bool currentlyInside;

        private Quaternion _closedRot;
        private Quaternion _openRot;

        private void Awake()
        {
            if (doorPivot == null) doorPivot = transform;
            _closedRot = doorPivot.localRotation;
            _openRot = _closedRot * Quaternion.Euler(0f, openAngle, 0f);
        }

        private void Update()
        {
            if (mode != DoorMode.ToggleDoorRotation || doorPivot == null) return;
            Quaternion target = isOpen ? _openRot : _closedRot;
            doorPivot.localRotation = Quaternion.Slerp(doorPivot.localRotation, target, 1f - Mathf.Exp(-swingSpeed * Time.deltaTime));
        }

        public override string Prompt
        {
            get
            {
                if (mode == DoorMode.ToggleDoorRotation)
                    return isOpen ? "Press E — Close Door" : "Press E — Open Door";
                return currentlyInside ? "Press E — Exit House" : "Press E — Enter House";
            }
        }

        protected override void OnInteract(GameObject interactor)
        {
            if (mode == DoorMode.ToggleDoorRotation)
            {
                isOpen = !isOpen;
                return;
            }

            var motor = interactor.GetComponentInParent<PlayerMotor>();
            if (motor == null) return;

            currentlyInside = !currentlyInside;
            Transform dest = currentlyInside ? interiorSpawn : exteriorSpawn;
            if (dest == null)
            {
                Debug.LogWarning($"HouseDoor on {name}: missing spawn transform.", this);
                currentlyInside = !currentlyInside;
                return;
            }

            motor.Teleport(dest.position, dest.rotation);
        }
    }
}
