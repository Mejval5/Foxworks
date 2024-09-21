using System;

namespace Foxworks.Utils
{
    public static class DateUtils
    {
        
        /// <summary>
        /// Returns true if the calling DateTime value is in-between the given dates, false otherwise.
        ///
        /// Note: All the dates must be set to the same `DateTimeKind`.
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool IsBetweenOrEqual(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }
    }
}