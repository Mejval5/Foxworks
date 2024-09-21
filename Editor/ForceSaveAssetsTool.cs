using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Fox
{
	public static class ForceSaveAssetsTool
	{
		[MenuItem("Assets/Reserialize Assets", false, -999)]
		public static void ReserializeAssets()
		{
			Object[] selectionObjects = Selection.objects;

			List<string> paths = new List<string>();
			for (int index = 0; index < selectionObjects.Length; ++index)
			{
				Object selectionObject = selectionObjects[index];
				if (selectionObject != null)
				{
					string path = AssetDatabase.GetAssetPath(selectionObject);
					paths.Add(path);
					if (Directory.Exists(path))
					{
						paths.AddRange(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(m => !m.EndsWith(".meta")).Select(m => m.Replace("\\", "/")));
						paths.AddRange(Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories).Select(m => m.Replace("\\", "/")));
					}
				}
			}

			AssetDatabase.ForceReserializeAssets(paths, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		[MenuItem("Tools/FoxUtils/Reserialize all Assets")]
		public static void ReserializeAllAssets()
		{
			AssetDatabase.ForceReserializeAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
	}
}