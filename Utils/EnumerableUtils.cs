
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fox.Utils
{
    public static class EnumerableUtils
    {
        public static T GetRandom<T>(this IEnumerable<T> source, T fallback)
        {
            List<T> list = source.ToList();
        
            int count = list.Count;
            if (count == 0)
                return fallback;

            int randIndex = Random.Range(0, count);
            return list.ElementAtOrDefault(randIndex);
        }
    
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Random.value);
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.OrderBy(x => Random.value).Take(count);
        }
    }
}
