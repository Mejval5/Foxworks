using System.Collections.Generic;
using Foxworks.External.SerializeInterfaces.Runtime;
using UnityEngine;

namespace Foxworks.External.SerializeInterfaces.Samples.Scripts
{
    public class InteractableTest : MonoBehaviour
    {
        // Arrays
        [RequireInterface(typeof(IInteractable))]
        public MonoBehaviour[] ReferenceWithAttributeArray;

        // Lists
        [RequireInterface(typeof(IInteractable))]
        public List<Object> ReferenceWithAttributeList;

        [RequireInterface(typeof(IInteractable))]
        public ScriptableObject AttributeRestrictedToScriptableObject;
        [RequireInterface(typeof(IInteractable))]
        public MonoBehaviour AttributeRestrictedToMonoBehaviour;

        public InterfaceReference<IInteractable>[] ReferenceArray;

        public List<InterfaceReference<IInteractable>> ReferenceList;
        public InterfaceReference<IInteractable, MonoBehaviour> ReferenceRestrictedToMonoBehaviour;

        // Fields
        public InterfaceReference<IInteractable, ScriptableObject> ReferenceRestrictedToScriptableObject;
    }
}