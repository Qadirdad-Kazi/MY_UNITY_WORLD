using UnityEngine;

namespace MyWorld.Vehicles
{
    /// <summary>
    /// Common API for all driveable vehicles.
    /// </summary>
    public abstract class VehicleControllerBase : MonoBehaviour
    {
        public bool IsPlayerDriving { get; private set; }

        public virtual void SetPlayerDriving(bool driving)
        {
            IsPlayerDriving = driving;
            enabled = driving;
        }

        protected float Horizontal => Input.GetAxis("Horizontal");
        protected float Vertical => Input.GetAxis("Vertical");
    }
}
