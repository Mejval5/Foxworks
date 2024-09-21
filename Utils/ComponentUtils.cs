using UnityEngine;

namespace Fox.Utils
{
    public static class ComponentUtils
    {
        public static void EnsureComponent<T>(this Component component, ref T output) where T : Component
        {
            if (!output && !component.TryGetComponent(out output))
            {
                output = component.gameObject.AddComponent<T>();
            }
        }
    
        public static void EnsureComponent<T>(this Component component) where T : Component
        {
            if (!component.TryGetComponent(out T _))
            {
                component.gameObject.AddComponent<T>();
            }
        }
    }
}