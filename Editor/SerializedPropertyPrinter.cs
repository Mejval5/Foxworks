using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
	/// <summary>
	/// Adds a context menu item on all GameObjects which allows
	/// the devs to print all serialized properties of all components attached.
	/// </summary>
	public static class SerializedPropertyPrinter
	{
		[MenuItem("GameObject/Print All Serialized Properties", false, 0)]
		public static void PrintAllSerializedProperties(MenuCommand command)
		{
			GameObject gameObject = command.context as GameObject;
			if (gameObject == null)
			{
				Debug.Log("No GameObject selected.");
				return;
			}

			foreach (Component component in gameObject.GetComponents<Component>())
			{
				Debug.Log("Component: " + component.GetType().Name);

				SerializedObject serializedObject = new SerializedObject(component);
				SerializedProperty iterator = serializedObject.GetIterator();

				if (iterator.NextVisible(true)) // Start with the first property
				{
					do
					{
						Debug.Log("Property: " + iterator.propertyPath);
					}
					while (iterator.NextVisible(false)); // Move to the next property
				}
			}
		}

		[MenuItem("GameObject/Print All Serialized Properties", true)]
		private static bool ValidatePrintAllSerializedProperties(MenuCommand command)
		{
			// The menu item will be disabled if no GameObject is selected
			return command.context is GameObject;
		}
	}
}