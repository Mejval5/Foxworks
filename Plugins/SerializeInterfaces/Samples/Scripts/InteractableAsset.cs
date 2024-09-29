using UnityEngine;

namespace Foxworks.External.SerializeInterfaces.Samples.Scripts
{
    public class InteractableAsset : ScriptableObject, IInteractable
    {
        public void Interact()
        {
            Debug.Log($"Interacted with asset: {name}");
        }
    }
}