using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
    /// <summary>
    /// Force save assets tool.
    /// Allows for reserializing assets to fix issues with assets that are not properly serialized.
    /// </summary>
    public static class ForceSaveAssetsTool
    {
        [MenuItem("Assets/Reserialize Assets", false, -999)]
        public static void ReserializeAssets()
        {
            Object[] selectionObjects = Selection.objects;

            List<string> paths = new();
            foreach (Object selectionObject in selectionObjects)
            {
                if (selectionObject == null)
                {
                    continue;
                }

                string path = AssetDatabase.GetAssetPath(selectionObject);
                paths.Add(path);
                if (!Directory.Exists(path))
                {
                    continue;
                }

                paths.AddRange(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(m => !m.EndsWith(".meta")).Select(m => m.Replace("\\", "/")));
                paths.AddRange(Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories).Select(m => m.Replace("\\", "/")));
            }

            AssetDatabase.ForceReserializeAssets(paths);
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