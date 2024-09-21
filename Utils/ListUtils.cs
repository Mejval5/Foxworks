using System;
using System.Collections.Generic;

namespace Foxworks.Utils
{
    public static class ListUtils
    {
        /// <summary>
        /// Swap two elements at the given indexes.
        ///
        /// Note: No swap is done if both elements are equal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
        {
            // Avoid useless swap
            if (firstIndex == secondIndex)
            {
                return;
            }

            // Swap elements
            (list[firstIndex], list[secondIndex]) = (list[secondIndex], list[firstIndex]);
        }
        
		/// <summary>
		/// Returns the median of the given list.
		///
		/// Note: The given list will be mutated in the process.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T Median<T>(this IList<T> list) where T : IComparable<T>
		{
			return list.GetNthSmallest((list.Count - 1) / 2);
		}

		/// <summary>
		/// Returns Nth smallest element from the list.
		///
		/// Note: The given list will be mutated in the process.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		private static T GetNthSmallest<T>(this IList<T> list, int n) where T : IComparable<T>
		{
			// Initialize indexes
			int start = 0;
			int end = list.Count - 1;
			int pivotIndex;

			do
			{
				// Get random pivot from the start/end range
				pivotIndex = list.Partition(start, end);

				// Update indexes
				if (n < pivotIndex)
				{
					end = pivotIndex - 1;
				}
				else
				{
					start = pivotIndex + 1;
				}
			} while (pivotIndex != n);

			return list[pivotIndex];
		}

		/// <summary>
		/// Partitions the given list around a pivot element such that all elements on left of pivot are equal or lower than the pivot
		/// and the ones at the right are greater than the pivot.
		/// This method can be used for sorting, N-order statistics such as median finding algorithms.
		///
		/// Note: Pivot is selected randomly.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		private static int Partition<T>(this IList<T> list, int start, int end) where T : IComparable<T>
		{
			// Select pivot randomly
			Random random = new Random();
			list.Swap(end, random.Next(start, end + 1));

			// Get pivot's value and initialize its index
			T pivotValue = list[end];
			int pivotIndex = start;

			// Move each element equal or lower than the pivot
			for (int i = start; i < end; i++)
			{
				if (list[i].CompareTo(pivotValue) <= 0)
				{
					list.Swap(i, pivotIndex++);
				}
			}

			list.Swap(end, pivotIndex);
			return pivotIndex;
		}
    }
}