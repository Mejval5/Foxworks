using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor.Utils
{
	public static class SelectionUtils
	{
		/// <summary>
		/// Returns all selected game objects.
		/// </summary>
		/// <param name="selectionMode"></param>
		/// <returns></returns>
		public static IEnumerable<GameObject> GetAllSelectedObjects(SelectionMode selectionMode = SelectionMode.Unfiltered)
		{
			Object[] selectionObjects = Selection.GetFiltered<Object>(selectionMode);

			List<GameObject> selectedGameObjects = new ();
			foreach (Object selectionObject in selectionObjects)
			{
				if (selectionObject == null)
				{
					continue;
				}

				GameObject selectionGameObject = selectionObject as GameObject;
				if (selectionGameObject == null)
				{
					continue;
				}

				selectedGameObjects.Add(selectionGameObject);
			}

			return selectedGameObjects;
		}
	}
}