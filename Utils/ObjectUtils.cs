using UnityEngine;

namespace Fox.Utils
{
    public static class ObjectUtils
    {
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
