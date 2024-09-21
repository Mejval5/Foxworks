using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
	public static class UnityEditorUtils
	{
        ///<summary>
        /// Pass through all the materials in project and check that they have valid shader selected
        ///</summary>
        [MenuItem("Tools/Search For Broken Materials", priority = 13)]
		public static void SearchForBrokenMaterials()
		{
			bool foundBrokenMaterial = false;

			string[] guids = AssetDatabase.FindAssets("t:material", new[] {"Assets"});

			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);

				Material material = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));

				if (material == null)
				{
					Debug.LogWarning($"Material at path {path} with guid {guid} is null.", material);
					continue;
				}

				bool isShaderBroken = material.shader == null
				                      || string.IsNullOrEmpty(material.shader.name)
				                      || material.shader.name == "Hidden/InternalErrorShader";

				if (isShaderBroken)
				{
					foundBrokenMaterial = true;
					Debug.LogWarning($"Material {material.name} has broken shader.", material);
				}
			}

			if (foundBrokenMaterial)
			{
				Debug.LogError("Found some materials with broken shaders, check warnings.");
			}

			Resources.UnloadUnusedAssets();
		}
	}
}