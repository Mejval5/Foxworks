using UnityEngine;

namespace Foxworks.Utils
{
    public static class ObjectUtils
    {
        /// <summary>
        ///     Checks if the object is null or destroyed.
        ///     This is useful when you want to check if an object is null or destroyed.
        ///     Unity's Objects are equal to null when they are destroyed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrDestroyed(this object obj)
        {
            return obj switch
            {
                null => true,
                Object o => o == null,
                _ => false
            };
        }
    }
}