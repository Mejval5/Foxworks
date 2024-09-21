using UnityEngine;

namespace Foxworks.Utils
{
    public static class ComponentUtils
    {
        /// <summary>
        ///     Ensures that a component exists on the GameObject.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="output"></param>
        /// <typeparam name="T"></typeparam>
        public static void EnsureComponent<T>(this Component component, ref T output) where T : Component
        {
            if (!output && !component.TryGetComponent(out output))
            {
                output = component.gameObject.AddComponent<T>();
            }
        }

        /// <summary>
        ///     Ensures that a component exists on the GameObject.
        /// </summary>
        /// <param name="component"></param>
        /// <typeparam name="T"></typeparam>
        public static void EnsureComponent<T>(this Component component) where T : Component
        {
            if (!component.TryGetComponent(out T _))
            {
                component.gameObject.AddComponent<T>();
            }
        }
    }
}