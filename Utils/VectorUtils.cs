using UnityEngine;

namespace Foxworks.Utils
{
    public static class VectorUtils
    {
        /// <summary>
        ///     Converts a Vector2 to a Vector3.
        ///     The z value is set to 0.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vec)
        {
            return new Vector3(vec.x, vec.y, 0f);
        }

        /// <summary>
        ///     Converts a Vector3 to a Vector2.
        ///     Ignores the z value.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}