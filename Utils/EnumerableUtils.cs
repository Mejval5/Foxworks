using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Foxworks.Utils
{
    public static class EnumerableUtils
    {
        /// <summary>
        ///     Gets a random element from the source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fallback"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> source, T fallback)
        {
            List<T> list = source.ToList();

            int count = list.Count;
            if (count == 0)
            {
                return fallback;
            }

            int randIndex = Random.Range(0, count);
            return list.ElementAtOrDefault(randIndex);
        }

        /// <summary>
        ///     Shuffles the source.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Random.value);
        }

        /// <summary>
        ///     Picks a random amount of elments from the source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.OrderBy(_ => Random.value).Take(count);
        }
        
        public static int IndexOfFirst<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (T item in collection)
            {
                if (predicate(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}