using System;

namespace Foxworks.Utils
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this string value, T fallbackValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return fallbackValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : fallbackValue;
        }
        
    }
}