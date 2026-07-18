using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Player;
using MyWorld.Interaction;
using MyWorld.World;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Enter/exit any vehicle. Works with PlayerMotor OR StarterAssets PlayerArmature.
    /// Requires PlayerInteraction on the player for Press E.
    /// </summary>
    [RequireComponent(typeof(VehicleSeat))]
    public class VehicleEnterExit : MonoBehaviour, IInteractable
    {
        [SerializeField] private string enterPrompt = "Press E — Drive";
        [SerializeField] private string exitPrompt = "Press E — Exit";
        [SerializeField] private VehicleControllerBase controller;
        [SerializeField] private float enterRadius = 3f;
        [Tooltip("Ignore exit for this long after enter (stops same-frame E from ejecting you).")]
        [SerializeField] private float exitLockSeconds = 0.45f;
        [SerializeField] private float exitGroundProbe = 4f;

        private VehicleSeat _seat;
        private Transform _driver;
        private PlayerMotor _driverMotor;
        private PlayerInteraction _driverInteraction;
        private CharacterController _driverCc;
        private Behaviour[] _disabledBehaviours;
        private bool _occupied;
        private float _exitUnlockTime;
        private CameraFollowTarget _camera;

        public string Prompt => _occupied ? exitPrompt : enterPrompt;

        private void Awake()
        {
            _seat = GetComponent<VehicleSeat>();
            if (controller == null) controller = GetComponent<VehicleControllerBase>();
            if (controller != null) controller.SetPlayerDriving(false);
        }

        private void Update()
        {
            if (!_occupied) return;
            if (Time.time < _exitUnlockTime) return;
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                ExitVehicle();
        }

        public bool CanInteract(GameObject interactor)
        {
            if (_occupied || interactor == null) return false;
            return GetPlayerRoot(interactor) != null;
        }

        public void Interact(GameObject interactor)
        {
            if (_occupied) return;
            var root = GetPlayerRoot(interactor);
            if (root == null) return;
            EnterVehicle(root);
        }

        private static Transform GetPlayerRoot(GameObject interactor)
        {
            var motor = interactor.GetComponentInParent<PlayerMotor>();
            if (motor != null) return motor.transform;

            var cc = interactor.GetComponentInParent<CharacterController>();
            if (cc != null) return cc.transform;

            if (interactor.CompareTag("Player")) return interactor.transform;
            var tagged = interactor.GetComponentInParent<Transform>();
            while (tagged != null)
            {
                if (tagged.CompareTag("Player")) return tagged;
                tagged = tagged.parent;
            }
            return null;
        }

        public void EnterVehicle(Transform playerRoot)
        {
            if (_occupied || playerRoot == null) return;

            _occupied = true;
            _exitUnlockTime = Time.time + exitLockSeconds;
            _driver = playerRoot;
            _driverMotor = playerRoot.GetComponent<PlayerMotor>();
            _driverInteraction = playerRoot.GetComponent<PlayerInteraction>();
            _driverCc = playerRoot.GetComponent<CharacterController>();

            if (_driverMotor != null) _driverMotor.SetMotorEnabled(false);
            if (_driverInteraction != null) _driverInteraction.SetInteractionEnabled(false);

            _disabledBehaviours = playerRoot.GetComponents<Behaviour>();
            foreach (var b in _disabledBehaviours)
            {
                if (b == null || b == _driverInteraction) continue;
                if (b is PlayerMotor) continue;
                string n = b.GetType().Name;
                if (n is "ThirdPersonController" or "StarterAssetsInputs" or "PlayerInput" or "BasicRigidBodyPush")
                    b.enabled = false;
            }

            if (_seat.HidePlayerWhileSeated)
            {
                foreach (var r in _driver.GetComponentsInChildren<Renderer>())
                    r.enabled = false;
                if (_driverCc != null) _driverCc.enabled = false;
            }

            _driver.SetParent(_seat.transform, false);
            _driver.localPosition = Vector3.zero;
            _driver.localRotation = Quaternion.identity;

            if (controller != null) controller.SetPlayerDriving(true);

            _camera = FindCamera();
            if (_camera != null)
                _camera.BeginDriving(transform, GetComponent<Rigidbody>());
        }

        public void ExitVehicle()
        {
            if (!_occupied || _driver == null) return;

            if (_camera != null)
            {
                _camera.EndDriving();
                _camera = null;
            }

            if (controller != null) controller.SetPlayerDriving(false);

            _driver.SetParent(null, true);
            Vector3 exitPos = ResolveExitPosition();
            Quaternion exitRot = _seat.ExitPoint.rotation;

            if (_seat.HidePlayerWhileSeated)
            {
                foreach (var r in _driver.GetComponentsInChildren<Renderer>())
                    r.enabled = true;
            }

            if (_driverCc != null) _driverCc.enabled = true;

            if (_driverMotor != null)
            {
                _driverMotor.Teleport(exitPos, exitRot);
                _driverMotor.SetMotorEnabled(true);
            }
            else
            {
                _driver.SetPositionAndRotation(exitPos, exitRot);
            }

            if (_disabledBehaviours != null)
            {
                foreach (var b in _disabledBehaviours)
                {
                    if (b == null) continue;
                    string n = b.GetType().Name;
                    if (n is "ThirdPersonController" or "StarterAssetsInputs" or "PlayerInput" or "BasicRigidBodyPush")
                        b.enabled = true;
                }
            }

            if (_driverInteraction != null) _driverInteraction.SetInteractionEnabled(true);

            _occupied = false;
            _driver = null;
            _driverMotor = null;
            _driverInteraction = null;
            _driverCc = null;
            _disabledBehaviours = null;
        }

        private Vector3 ResolveExitPosition()
        {
            Vector3 exitPos = _seat.ExitPoint.position;
            // Snap to ground so uneven terrain does not bury or float the player
            Vector3 probe = exitPos + Vector3.up * 2f;
            if (Physics.Raycast(probe, Vector3.down, out RaycastHit hit, exitGroundProbe + 2f, ~0, QueryTriggerInteraction.Ignore))
                exitPos = hit.point + Vector3.up * 0.15f;
            return exitPos;
        }

        private static CameraFollowTarget FindCamera()
        {
            if (Camera.main != null)
            {
                var onMain = Camera.main.GetComponent<CameraFollowTarget>();
                if (onMain != null) return onMain;
            }
            return Object.FindFirstObjectByType<CameraFollowTarget>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enterRadius);
        }
#endif
    }
}
