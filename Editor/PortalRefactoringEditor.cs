using Editor.Fox.Utils;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Editor.Fox
{
	using SelectionMode = SelectionProcessingUtils.SelectionMode;

	public class PortalRefactoringEditor : RefactoringEditorBase
    {
		private Material _portalMaterial;
		private int _portalsLayer;
		private string _filterName;

		[Shortcut("Tools/Portal Refactoring Editor", KeyCode.T, ShortcutModifiers.Control)]
		[MenuItem("Tools/Portal Refactoring Editor", false, 20)]
		public static void Init()
		{
			EditorWindow window = GetWindow<PortalRefactoringEditor>("Portal Refactoring Editor");
			window.minSize = new Vector2(WindowWidth, WindowHeight);
			window.Show();
		}

		protected override void DrawCustomGUI()
		{
			_portalMaterial = (Material)EditorGUILayout.ObjectField("Portal Material", _portalMaterial, typeof(Material), true);
			
			EditorUtils.ShowHorizontalLine();
			
			_portalsLayer = EditorGUILayout.LayerField("Portals layer", _portalsLayer);

			// Button to load
			if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\n layer based on material."))
			{
				SelectionProcessingUtils.DecideOnHowToProcessSelection(ReplaceAllSelectedObjectsLayerByMaterial, _selectionMode);
			}
		}

		private void ReplaceAllSelectedObjectsLayerByMaterial(SelectionMode selectionMode)
		{
			foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetSelectedPlainGameObjects(selectionMode, _replacePrefabs))
			{
				if (selectionGameObject.TryGetComponent(out MeshRenderer meshRenderer) == false)
				{
					continue;
				}

				if (meshRenderer.sharedMaterial == _portalMaterial)
				{
					selectionGameObject.layer = _portalsLayer;
				}

			}
		}
    }
}