using UnityEngine;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    public abstract class VehicleControllerBase : MonoBehaviour
    {
        public bool IsPlayerDriving { get; private set; }

        /// <summary>True when mesh faces opposite transform.forward (W uses Invert Throttle).</summary>
        public virtual bool InvertDriveForward => false;

        public virtual void SetPlayerDriving(bool driving)
        {
            IsPlayerDriving = driving;
            enabled = driving;
        }

        protected float Horizontal => GameInput.Horizontal;
        protected float Vertical => GameInput.Vertical;
    }
}
