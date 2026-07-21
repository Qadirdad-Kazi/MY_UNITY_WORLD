using UnityEngine;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Seated / bike-straddle pose without Mixamo. Uses Humanoid bones when available
    /// (Starter Assets = Left_UpperLeg etc.).
    /// </summary>
    public class DriverSitPose : MonoBehaviour
    {
        [SerializeField] private Transform hips;
        [SerializeField] private Transform spine;
        [SerializeField] private Transform chest;
        [SerializeField] private Transform leftUpperLeg;
        [SerializeField] private Transform rightUpperLeg;
        [SerializeField] private Transform leftLowerLeg;
        [SerializeField] private Transform rightLowerLeg;
        [SerializeField] private float hipsPitchCar = 40f;
        [SerializeField] private float hipsPitchBike = 25f;
        [SerializeField] private float spinePitchCar = 12f;
        [SerializeField] private float spinePitchBike = 22f;
        [SerializeField] private float legPitchCar = -75f;
        [SerializeField] private float legPitchBike = -55f;
        [SerializeField] private float lowerLegPitchCar = 70f;
        [SerializeField] private float lowerLegPitchBike = 45f;

        private bool _active;
        private VehicleKind _kind;
        private Animator _animator;
        private bool _hadAnimatorEnabled;
        private Quaternion _hips0, _spine0, _chest0, _lLeg0, _rLeg0, _lLower0, _rLower0;
        private bool _cached;

        public void Begin(VehicleKind kind, Animator animator, string sitStateName, float crossFade)
        {
            _kind = kind;
            _animator = animator;
            _cached = false;
            CacheBones();

            if (_animator != null && !string.IsNullOrEmpty(sitStateName))
            {
                _hadAnimatorEnabled = _animator.enabled;
                _animator.enabled = true;
                _animator.applyRootMotion = false;
                if (_animator.HasState(0, Animator.StringToHash(sitStateName)))
                    _animator.CrossFadeInFixedTime(sitStateName, crossFade);
                else
                    _animator.Play(sitStateName, 0, 0f);
                _active = false;
                enabled = true;
                return;
            }

            if (_animator != null)
            {
                _hadAnimatorEnabled = _animator.enabled;
                _animator.enabled = false;
            }

            _active = true;
            enabled = true;
            ApplyPose();
        }

        public void End()
        {
            _active = false;
            RestoreBones();
            if (_animator != null)
            {
                _animator.enabled = _hadAnimatorEnabled;
                _animator = null;
            }
            enabled = false;
        }

        private void LateUpdate()
        {
            if (_active) ApplyPose();
        }

        private void ApplyPose()
        {
            bool bike = _kind == VehicleKind.Bike;
            float hipsPitch = bike ? hipsPitchBike : hipsPitchCar;
            float spinePitch = bike ? spinePitchBike : spinePitchCar;
            float legPitch = bike ? legPitchBike : legPitchCar;
            float lowerPitch = bike ? lowerLegPitchBike : lowerLegPitchCar;

            if (hips != null)
                hips.localRotation = _hips0 * Quaternion.Euler(hipsPitch, 0f, 0f);
            if (spine != null)
                spine.localRotation = _spine0 * Quaternion.Euler(spinePitch, 0f, 0f);
            if (chest != null)
                chest.localRotation = _chest0 * Quaternion.Euler(bike ? 8f : 4f, 0f, 0f);
            if (leftUpperLeg != null)
                leftUpperLeg.localRotation = _lLeg0 * Quaternion.Euler(legPitch, bike ? -12f : 0f, bike ? 18f : 0f);
            if (rightUpperLeg != null)
                rightUpperLeg.localRotation = _rLeg0 * Quaternion.Euler(legPitch, bike ? 12f : 0f, bike ? -18f : 0f);
            if (leftLowerLeg != null)
                leftLowerLeg.localRotation = _lLower0 * Quaternion.Euler(lowerPitch, 0f, 0f);
            if (rightLowerLeg != null)
                rightLowerLeg.localRotation = _rLower0 * Quaternion.Euler(lowerPitch, 0f, 0f);
        }

        private void CacheBones()
        {
            if (_cached) return;

            if (_animator != null && _animator.isHuman)
            {
                if (hips == null) hips = _animator.GetBoneTransform(HumanBodyBones.Hips);
                if (spine == null) spine = _animator.GetBoneTransform(HumanBodyBones.Spine);
                if (chest == null) chest = _animator.GetBoneTransform(HumanBodyBones.Chest);
                if (leftUpperLeg == null) leftUpperLeg = _animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
                if (rightUpperLeg == null) rightUpperLeg = _animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                if (leftLowerLeg == null) leftLowerLeg = _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
                if (rightLowerLeg == null) rightLowerLeg = _animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            }

            if (hips == null) hips = FindBoneExact("Hips", "Pelvis");
            if (spine == null) spine = FindBoneExact("Spine");
            if (chest == null) chest = FindBoneExact("Chest", "UpperChest");
            if (leftUpperLeg == null) leftUpperLeg = FindBoneExact("Left_UpperLeg", "LeftUpLeg", "LeftUpperLeg");
            if (rightUpperLeg == null) rightUpperLeg = FindBoneExact("Right_UpperLeg", "RightUpLeg", "RightUpperLeg");
            if (leftLowerLeg == null) leftLowerLeg = FindBoneExact("Left_LowerLeg", "LeftLowerLeg", "LeftLeg");
            if (rightLowerLeg == null) rightLowerLeg = FindBoneExact("Right_LowerLeg", "RightLowerLeg", "RightLeg");

            if (hips != null) _hips0 = hips.localRotation;
            if (spine != null) _spine0 = spine.localRotation;
            if (chest != null) _chest0 = chest.localRotation;
            if (leftUpperLeg != null) _lLeg0 = leftUpperLeg.localRotation;
            if (rightUpperLeg != null) _rLeg0 = rightUpperLeg.localRotation;
            if (leftLowerLeg != null) _lLower0 = leftLowerLeg.localRotation;
            if (rightLowerLeg != null) _rLower0 = rightLowerLeg.localRotation;
            _cached = true;
        }

        private void RestoreBones()
        {
            if (!_cached) return;
            if (hips != null) hips.localRotation = _hips0;
            if (spine != null) spine.localRotation = _spine0;
            if (chest != null) chest.localRotation = _chest0;
            if (leftUpperLeg != null) leftUpperLeg.localRotation = _lLeg0;
            if (rightUpperLeg != null) rightUpperLeg.localRotation = _rLeg0;
            if (leftLowerLeg != null) leftLowerLeg.localRotation = _lLower0;
            if (rightLowerLeg != null) rightLowerLeg.localRotation = _rLower0;
        }

        private Transform FindBoneExact(params string[] names)
        {
            var all = GetComponentsInChildren<Transform>(true);
            foreach (var key in names)
            {
                foreach (var t in all)
                {
                    if (t.name.Equals(key, System.StringComparison.OrdinalIgnoreCase))
                        return t;
                }
            }
            return null;
        }
    }
}
