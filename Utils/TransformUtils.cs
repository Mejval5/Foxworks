using UnityEngine;

namespace Foxworks.Utils
{
    public static class TransformUtils
    {
        /// <summary>
        ///     Destroys all children of the transform.
        ///     This is useful when you want to clear the children of a transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}