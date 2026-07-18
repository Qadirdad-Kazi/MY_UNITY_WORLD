using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Follows a target transform (use PlayerCameraRoot on StarterAssets PlayerArmature).
    /// No mouse look here — ThirdPersonController already rotates PlayerCameraRoot.
    /// </summary>
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [Tooltip("Local offset from PlayerCameraRoot. Z negative = behind player (third person).")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 0.5f, -4f);
        [SerializeField] private float positionSharpness = 12f;
        [SerializeField] private bool matchRotation = true;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                1f - Mathf.Exp(-positionSharpness * Time.deltaTime));

            if (matchRotation)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    target.rotation,
                    1f - Mathf.Exp(-positionSharpness * Time.deltaTime));
        }

        public void SetTarget(Transform t) => target = t;
    }
}
