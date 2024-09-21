using UnityEngine;

namespace Fox.Utils
{
    public static class VectorUtils
    {
        public static Vector3 ToVector3(this Vector2 vec)
        {
            return new Vector3(vec.x, vec.y, 0f);
        }
    
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}