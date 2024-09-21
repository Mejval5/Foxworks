using UnityEngine;

namespace Foxworks.External.SerializeInterfaces.Samples.Scripts
{
    public class InteractableComponent : MonoBehaviour, IInteractable
    {
        public string DebugText;

        public void Interact()
        {
            Debug.Log($"Interacted with component: {name}");
        }
    }
}