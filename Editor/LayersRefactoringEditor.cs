using Foxworks.Editor.Utils;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Foxworks.Editor
{
    using SelectionMode = SelectionProcessingUtils.SelectionMode;

    /// <summary>
    /// Useful for refactoring layers based on materials.
    /// </summary>
    public class LayersRefactoringEditor : RefactoringEditorBase
    {
        private string _filterName;
        private Material _targetMaterial;
        private int _targetLayer;

        [Shortcut("Tools/Layers Refactoring Editor", KeyCode.T, ShortcutModifiers.Control)]
        [MenuItem("Tools/Layers Refactoring Editor", false, 20)]
        public static void Init()
        {
            EditorWindow window = GetWindow<LayersRefactoringEditor>("Layers Refactoring Editor");
            window.minSize = new Vector2(WindowWidth, WindowHeight);
            window.Show();
        }

        protected override void DrawCustomGUI()
        {
            _targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", _targetMaterial, typeof(Material), true);

            EditorUtils.ShowHorizontalLine();

            _targetLayer = EditorGUILayout.LayerField("Target layer", _targetLayer);

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

                if (meshRenderer.sharedMaterial == _targetMaterial)
                {
                    selectionGameObject.layer = _targetLayer;
                }
            }
        }
    }
}