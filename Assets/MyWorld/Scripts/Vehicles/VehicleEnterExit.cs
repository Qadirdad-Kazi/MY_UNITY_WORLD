using UnityEngine;
using UnityEngine.InputSystem;
using MyWorld.Player;
using MyWorld.Interaction;
using MyWorld.World;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Enter/exit vehicles. Exit by holding E while driving.
    /// </summary>
    [RequireComponent(typeof(VehicleSeat))]
    public class VehicleEnterExit : MonoBehaviour, IInteractable
    {
        [SerializeField] private string enterPrompt = "Press E — Drive";
        [SerializeField] private string exitPrompt = "Hold E to exit";
        [SerializeField] private VehicleControllerBase controller;
        [SerializeField] private float enterRadius = 3f;
        [SerializeField] private float exitLockSeconds = 0.35f;
        [SerializeField] private float exitHoldSeconds = 0.45f;
        [SerializeField] private float exitGroundProbe = 12f;

        private VehicleSeat _seat;
        private Transform _driver;
        private PlayerMotor _driverMotor;
        private PlayerInteraction _driverInteraction;
        private PlayerSwim _driverSwim;
        private CharacterController _driverCc;
        private Animator _driverAnimator;
        private DriverSitPose _sitPose;
        private Behaviour[] _disabledBehaviours;
        private bool _occupied;
        private float _exitUnlockTime;
        private CameraFollowTarget _camera;
        private Vector3 _driverScale;
        private VehicleExitBridge _exitBridge;

        public string Prompt => _occupied ? exitPrompt : enterPrompt;
        public bool IsOccupied => _occupied;
        public bool CanExitNow => _occupied && Time.unscaledTime >= _exitUnlockTime;
        public float ExitHoldSeconds => Mathf.Clamp(exitHoldSeconds, 0.2f, 2f);

        private void Awake()
        {
            _seat = GetComponent<VehicleSeat>();
            if (controller == null) controller = GetComponent<VehicleControllerBase>();
            if (controller != null) controller.SetPlayerDriving(false);

            if (GetComponent<Collider>() == null && GetComponentInChildren<Collider>() == null)
            {
                var box = gameObject.AddComponent<BoxCollider>();
                box.size = new Vector3(1.2f, 1.2f, 2.2f);
                box.center = new Vector3(0f, 0.6f, 0f);
            }
        }

        public static bool ExitKeyHeld()
        {
            var kb = Keyboard.current;
            return kb != null && kb.eKey.isPressed;
        }

        public bool CanInteract(GameObject interactor)
        {
            if (_occupied || interactor == null) return false;
            Transform root = GetPlayerRoot(interactor);
            if (root == null) return false;

            float r = enterRadius > 0.1f ? enterRadius : 3f;
            Vector3 delta = root.position - transform.position;
            delta.y = 0f;
            return delta.sqrMagnitude <= r * r;
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
            var tagged = interactor.transform;
            while (tagged != null)
            {
                if (tagged.CompareTag("Player")) return tagged;
                tagged = tagged.parent;
            }
            return null;
        }

        private static PlayerInteraction FindInteraction(Transform root)
        {
            if (root == null) return null;
            var pi = root.GetComponent<PlayerInteraction>();
            if (pi != null) return pi;
            pi = root.GetComponentInChildren<PlayerInteraction>(true);
            if (pi != null) return pi;
            return root.GetComponentInParent<PlayerInteraction>();
        }

        public void EnterVehicle(Transform playerRoot)
        {
            if (_occupied || playerRoot == null) return;

            _occupied = true;
            float lockSec = Mathf.Clamp(exitLockSeconds, 0.05f, 1f);
            _exitUnlockTime = Time.unscaledTime + lockSec;
            _driver = playerRoot;
            _driverScale = playerRoot.localScale;
            _driverMotor = playerRoot.GetComponent<PlayerMotor>();
            _driverInteraction = FindInteraction(playerRoot);
            _driverSwim = playerRoot.GetComponentInChildren<PlayerSwim>(true)
                ?? playerRoot.GetComponentInParent<PlayerSwim>();
            _driverCc = playerRoot.GetComponent<CharacterController>()
                ?? playerRoot.GetComponentInChildren<CharacterController>();
            _driverAnimator = playerRoot.GetComponentInChildren<Animator>();

            if (_driverMotor != null) _driverMotor.SetMotorEnabled(false);
            if (_driverSwim != null) _driverSwim.SetSwimAllowed(false);

            // Keep PlayerInput ENABLED — disabling it can stop Keyboard.current on Input System Only projects.
            _disabledBehaviours = playerRoot.GetComponents<Behaviour>();
            foreach (var b in _disabledBehaviours)
            {
                if (b == null) continue;
                if (b is PlayerMotor or PlayerSwim or PlayerInteraction) continue;
                string n = b.GetType().Name;
                if (n is "ThirdPersonController" or "StarterAssetsInputs" or "BasicRigidBodyPush")
                    b.enabled = false;
            }

            if (_driverInteraction != null)
            {
                _driverInteraction.SetInteractionEnabled(false);
                _driverInteraction.SetDrivingVehicle(this);
            }

            _exitBridge = playerRoot.GetComponent<VehicleExitBridge>();
            if (_exitBridge == null) _exitBridge = playerRoot.gameObject.AddComponent<VehicleExitBridge>();
            _exitBridge.Bind(this);

            if (_driverCc != null) _driverCc.enabled = false;

            if (_seat == null) _seat = GetComponent<VehicleSeat>();
            Transform seatT = _seat != null ? _seat.SitTransform : transform;
            float hipDrop = _seat != null && _seat.AutoDropHipsToSeat
                ? EstimateHipHeight(_driver, _driverAnimator, _driverCc)
                : 0f;
            Vector3 worldPos = seatT.position
                + seatT.rotation * (_seat != null ? _seat.SeatLocalOffset : Vector3.zero)
                - Vector3.up * hipDrop;
            Quaternion worldRot = seatT.rotation * (_seat != null ? _seat.SeatLocalRotation : Quaternion.identity);

            _driver.SetParent(seatT, true);
            _driver.SetPositionAndRotation(worldPos, worldRot);
            _driver.localScale = CompensateParentScale(seatT, _driverScale)
                * (_seat != null ? _seat.SeatedScaleMultiplier : 1f);

            if (_seat != null && _seat.HidePlayerWhileSeated)
            {
                foreach (var r in _driver.GetComponentsInChildren<Renderer>())
                    r.enabled = false;
            }
            else
            {
                _sitPose = _driver.GetComponent<DriverSitPose>();
                if (_sitPose == null) _sitPose = _driver.gameObject.AddComponent<DriverSitPose>();
                _sitPose.Begin(
                    _seat != null ? _seat.Kind : VehicleKind.Car,
                    _driverAnimator,
                    _seat != null ? _seat.SitStateName : "",
                    _seat != null ? _seat.SitCrossFade : 0.15f);
            }

            if (controller != null) controller.SetPlayerDriving(true);

            _camera = FindCamera();
            if (_camera != null)
            {
                bool invertCam = (_seat != null && _seat.InvertChaseCamera)
                    || (controller != null && controller.InvertDriveForward);
                _camera.BeginDriving(transform, GetComponent<Rigidbody>(), invertCam);
            }
        }

        /// <param name="force">Skip the short enter-lock (used by hold-E / bridge).</param>
        public void ExitVehicle(bool force = false)
        {
            if (!_occupied || _driver == null) return;
            if (!force && !CanExitNow) return;

            try
            {
                if (_camera != null)
                {
                    _camera.EndDriving();
                    _camera = null;
                }

                if (controller != null) controller.SetPlayerDriving(false);

                if (_sitPose != null)
                {
                    _sitPose.End();
                    _sitPose = null;
                }

                if (_seat != null && _seat.HidePlayerWhileSeated)
                {
                    foreach (var r in _driver.GetComponentsInChildren<Renderer>())
                        r.enabled = true;
                }

                _driver.SetParent(null, true);
                _driver.localScale = _driverScale;

                Vector3 exitPos = ResolveExitPosition();
                Quaternion exitRot = _seat != null ? _seat.ExitPoint.rotation : transform.rotation;

                if (_driverCc != null) _driverCc.enabled = false;
                _driver.SetPositionAndRotation(exitPos, exitRot);
                if (_driverCc != null) _driverCc.enabled = true;

                if (_driverMotor != null)
                {
                    _driverMotor.Teleport(exitPos, exitRot);
                    _driverMotor.SetMotorEnabled(true);
                }

                if (_disabledBehaviours != null)
                {
                    foreach (var b in _disabledBehaviours)
                    {
                        if (b == null) continue;
                        string n = b.GetType().Name;
                        if (n is "ThirdPersonController" or "StarterAssetsInputs" or "BasicRigidBodyPush")
                            b.enabled = true;
                    }
                }

                if (_driverSwim != null) _driverSwim.SetSwimAllowed(true);

                if (_exitBridge != null)
                {
                    _exitBridge.Unbind();
                    _exitBridge = null;
                }

                if (_driverInteraction != null)
                {
                    _driverInteraction.SetDrivingVehicle(null);
                    _driverInteraction.SetInteractionEnabled(true);
                }
            }
            finally
            {
                _occupied = false;
                _driver = null;
                _driverMotor = null;
                _driverInteraction = null;
                _driverSwim = null;
                _driverCc = null;
                _driverAnimator = null;
                _disabledBehaviours = null;
                _exitBridge = null;
            }
        }

        private Vector3 ResolveExitPosition()
        {
            Transform exit = _seat != null ? _seat.ExitPoint : transform;
            Vector3 exitPos = exit != null ? exit.position : transform.position + transform.right * 2.2f;

            if (exit == transform || exit == null)
                exitPos = transform.position + transform.right * 2.2f;

            Vector3 probe = exitPos + Vector3.up * 3f;
            if (Physics.Raycast(probe, Vector3.down, out RaycastHit hit, exitGroundProbe + 3f, ~0, QueryTriggerInteraction.Ignore))
                exitPos = hit.point + Vector3.up * 0.15f;
            else
            {
                Vector3 fromVehicle = transform.position + Vector3.up * 3f;
                if (Physics.Raycast(fromVehicle, Vector3.down, out RaycastHit hit2, exitGroundProbe + 3f, ~0, QueryTriggerInteraction.Ignore))
                    exitPos = hit2.point + transform.right * 2.2f + Vector3.up * 0.15f;
                else
                    exitPos = new Vector3(exitPos.x, transform.position.y + 0.2f, exitPos.z);
            }

            return exitPos;
        }

        private static float EstimateHipHeight(Transform root, Animator animator, CharacterController cc)
        {
            if (animator != null && animator.isHuman)
            {
                Transform hips = animator.GetBoneTransform(HumanBodyBones.Hips);
                if (hips != null)
                {
                    float h = root.InverseTransformPoint(hips.position).y;
                    if (h > 0.2f) return h;
                }
            }

            if (cc != null && cc.height > 0.5f)
                return cc.height * 0.55f;

            return 0.95f;
        }

        private static Vector3 CompensateParentScale(Transform parent, Vector3 desiredWorldScale)
        {
            Vector3 p = parent.lossyScale;
            return new Vector3(
                Div(desiredWorldScale.x, p.x),
                Div(desiredWorldScale.y, p.y),
                Div(desiredWorldScale.z, p.z));
        }

        private static float Div(float a, float b)
        {
            float abs = Mathf.Abs(b);
            if (abs < 0.0001f) return a;
            return a / b;
        }

        private static CameraFollowTarget FindCamera()
        {
            if (Camera.main != null)
            {
                var onMain = Camera.main.GetComponent<CameraFollowTarget>();
                if (onMain != null) return onMain;
            }
            return Object.FindAnyObjectByType<CameraFollowTarget>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enterRadius);
        }
#endif
    }

    /// <summary>
    /// On the player while driving: hold E to exit. Shows a simple prompt.
    /// </summary>
    public class VehicleExitBridge : MonoBehaviour
    {
        private VehicleEnterExit _vehicle;
        private float _holdE;

        public void Bind(VehicleEnterExit vehicle)
        {
            _vehicle = vehicle;
            _holdE = 0f;
            enabled = true;
        }

        public void Unbind()
        {
            _vehicle = null;
            _holdE = 0f;
            enabled = false;
        }

        private void Update()
        {
            if (_vehicle == null || !_vehicle.IsOccupied)
            {
                enabled = false;
                return;
            }

            if (!_vehicle.CanExitNow)
            {
                _holdE = 0f;
                return;
            }

            if (VehicleEnterExit.ExitKeyHeld())
            {
                _holdE += Time.unscaledDeltaTime;
                if (_holdE >= _vehicle.ExitHoldSeconds)
                {
                    _holdE = 0f;
                    _vehicle.ExitVehicle(force: true);
                }
            }
            else
            {
                _holdE = 0f;
            }
        }

        private void OnGUI()
        {
            if (_vehicle == null || !_vehicle.IsOccupied || !_vehicle.CanExitNow) return;
            var rect = new Rect(Screen.width * 0.5f - 80f, Screen.height * 0.75f, 160f, 28f);
            GUI.Box(rect, "Hold E to exit");
        }
    }
}
