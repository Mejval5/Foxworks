using System;
using System.Collections.Generic;
using System.Linq;

namespace Foxworks.Utils
{
    public static class DictionaryUtils
    {
        
        public static Dictionary<TKey, TValue> MergeLeft<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params Dictionary<TKey, TValue>[] otherDictionaries)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> x in dictionary)
            {
                result[x.Key] = x.Value;
            }

            if (otherDictionaries == null)
            {
                return result;
            }

            {
                foreach (Dictionary<TKey, TValue> dict in otherDictionaries)
                {
                    if (dict == null)
                    {
                        continue;
                    }

                    foreach (KeyValuePair<TKey, TValue> x in dict)
                    {
                        result[x.Key] = x.Value;
                    }
                }
            }

            return result;
        }
        
		/// <summary>
		/// Allows dictionaries to be initialised with a list of KeyValuePairs.
		/// </summary>
		public static void Add<T0, T1>(this IDictionary<T0, T1> dict, KeyValuePair<T0, T1> item)
		{
			dict.Add(item.Key, item.Value);
		}

		public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}

			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				target.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		/// <summary>
		/// Allows dictionaries to be initialised with a key and a list of values.
		/// </summary>
		public static void Add<T0, T1>(this IDictionary<T0, List<T1>> dict, T0 key, params T1[] values)
		{
			dict.Add(key, values.ToList());
		}

		/// <summary>
		/// Returns the value with the associated key. Adds the value first if not already in the dictionary.
		/// </summary>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			return dict.GetOrAdd(key, new TValue());
		}

		/// <summary>
		/// Returns the value with the associated key. Adds the value first if not already in the dictionary.
		/// </summary>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
		{
			if (dict.ContainsKey(key) == false)
			{
				dict.Add(key, defaultValue);
			}

			return dict[key];
		}
    }
}