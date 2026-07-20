using UnityEngine;

namespace MyWorld.World
{
    /// <summary>
    /// Simple underwater fish motion. Place FishV1–V4 prefabs below water surface.
    /// </summary>
    public class FishSwim : MonoBehaviour
    {
        [SerializeField] private float swimSpeed = 1.2f;
        [SerializeField] private float turnSpeed = 0.8f;
        [SerializeField] private float wanderRadius = 6f;
        [SerializeField] private float depthLockY = -999f;
        [SerializeField] private float bobAmount = 0.15f;

        private Vector3 _home;
        private Vector3 _target;
        private float _bobPhase;

        private void Start()
        {
            _home = transform.position;
            if (depthLockY < -900f) depthLockY = _home.y;
            PickNewTarget();
        }

        private void Update()
        {
            _bobPhase += Time.deltaTime * 2f;
            Vector3 pos = transform.position;
            pos += ( _target - pos).normalized * (swimSpeed * Time.deltaTime);
            pos.y = depthLockY + Mathf.Sin(_bobPhase) * bobAmount;
            transform.position = pos;

            Vector3 look = _target - pos;
            look.y = 0f;
            if (look.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }

            if (( _target - pos).sqrMagnitude < 0.5f)
                PickNewTarget();
        }

        private void PickNewTarget()
        {
            Vector2 r = Random.insideUnitCircle * wanderRadius;
            _target = _home + new Vector3(r.x, 0f, r.y);
        }
    }
}
