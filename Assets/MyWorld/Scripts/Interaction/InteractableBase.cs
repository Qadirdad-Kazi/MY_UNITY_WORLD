using UnityEngine;

namespace MyWorld.Interaction
{
    /// <summary>
    /// Base for doors, chests, seats, etc. Add a Collider (Is Trigger optional).
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private string prompt = "Press E";
        [SerializeField] private bool oneShot;
        [SerializeField] private bool consumed;

        public virtual string Prompt => prompt;

        public virtual bool CanInteract(GameObject interactor)
        {
            return isActiveAndEnabled && !consumed && interactor != null;
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor)) return;
            OnInteract(interactor);
            if (oneShot) consumed = true;
        }

        protected abstract void OnInteract(GameObject interactor);
    }
}
