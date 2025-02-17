using UnityEngine;

namespace Foxworks.Utils
{
    public static class VectorUtils
    {
        public static Vector3 SetX(this Vector3 vec, float x)
        {
            vec.x = x;
            return vec;
        }
        
        public static Vector3 SetY(this Vector3 vec, float y)
        {
            vec.y = y;
            return vec;
        }
        
        public static Vector3 SetZ(this Vector3 vec, float z)
        {
            vec.z = z;
            return vec;
        }
        
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
        
        /// <summary>
        /// Rotates a vector by the given angle in degrees.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="degrees">The rotation angle in degrees.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);
            return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
        }
    }
}