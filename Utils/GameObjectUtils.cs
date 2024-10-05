using UnityEngine;

namespace Foxworks.Utils
{
    public static class GameObjectUtils
    {
        /// <summary>
        ///     Destroys all children of the Game Object.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyAllChildren();
        }
    }
}