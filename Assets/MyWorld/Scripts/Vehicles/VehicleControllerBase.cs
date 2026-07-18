using UnityEngine;
using MyWorld.Core;

namespace MyWorld.Vehicles
{
    public abstract class VehicleControllerBase : MonoBehaviour
    {
        public bool IsPlayerDriving { get; private set; }

        public virtual void SetPlayerDriving(bool driving)
        {
            IsPlayerDriving = driving;
            enabled = driving;
        }

        protected float Horizontal => GameInput.Horizontal;
        protected float Vertical => GameInput.Vertical;
    }
}
