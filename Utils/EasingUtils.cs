using UnityEngine;

namespace Foxworks.Utils
{
    public static class EasingUtils
    {
        /// <summary>
        /// Simple cubic easing in function.
        /// </summary>
        public static float EaseInOutCubic(float x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }
        
        /// <summary>
        /// Simple cubic easing out function.
        /// </summary>
        public static float EaseOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }
        
        /// <summary>
        /// Simple cubic easing in function.
        /// </summary>
        public static float EaseInCubic(float x)
        {
            return x * x * x;
        }
    }
}