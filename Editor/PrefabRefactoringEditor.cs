using System.Collections.Generic;
using System.Linq;
using Foxworks.Editor.Utils;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Foxworks.Editor
{
    using SelectionMode = SelectionProcessingUtils.SelectionMode;

    /// <summary>
    ///  Alows to bulk edit prefabs.
    ///  Useful for replacing prefabs based on filters.
    /// </summary>
    public class PrefabRefactoringEditor : RefactoringEditorBase
    {
        private bool _enableShadowCasting;
        private Mesh _filterMesh;
        private string _filterName;
        private ShadowProcessing _shadowProcessing;

        private GameObject _targetPrefab;

        [Shortcut("Tools/Prefab Refactoring Editor", KeyCode.R, ShortcutModifiers.Control)]
        [MenuItem("Tools/Prefab Refactoring Editor", false, 20)]
        public static void Init()
        {
            EditorWindow window = GetWindow<PrefabRefactoringEditor>("Prefab Refactoring Editor");
            window.minSize = new Vector2(WindowWidth, WindowHeight);
            window.Show();
        }

        protected override void DrawCustomGUI()
        {
            _targetPrefab = (GameObject)EditorGUILayout.ObjectField("Target Prefab", _targetPrefab, typeof(GameObject), true);

            EditorUtils.ShowHorizontalLine();

            _filterMesh = (Mesh)EditorGUILayout.ObjectField("Filter Mesh", _filterMesh, typeof(Mesh), true);

            // Button to load
            if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\nby prefab (filter by mesh)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(ReplaceAllSelectedObjectsByPrefabFilterByMesh, _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            _filterName = EditorGUILayout.TextField("Filter Name", _filterName);

            // Button to load
            if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\nby prefab (filter by name)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(replaceBy => ReplaceAllSelectedObjectsByPrefabFilterByName(_filterName, replaceBy), _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            // Button to load
            if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\nby prefab (filter by name of prefab)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(replaceBy => ReplaceAllSelectedObjectsByPrefabFilterByName(_targetPrefab.name, replaceBy), _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            // Button to load
            if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\nby prefab (filter by same components)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(ReplaceAllSelectedObjectsByPrefabFilterByComponents, _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            // Button to load
            if (GUILayout.Button($"Replace {_selectionMode.ToPrint()}\nby prefab (no filter)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(ReplaceAllSelectedObjectsByPrefab, _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            GUILayout.BeginHorizontal();

            _enableShadowCasting = EditorGUILayout.Toggle("Enable Shadow Casting", _enableShadowCasting);

            _shadowProcessing = (ShadowProcessing)EditorGUILayout.EnumPopup(_shadowProcessing);

            GUILayout.EndHorizontal();

            string shadowCastingButtonLabel = _enableShadowCasting ? "Enable" : "Disable";

            // Button to load
            if (GUILayout.Button($"{shadowCastingButtonLabel} shadow processing\nin {_selectionMode.ToPrint()}"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(replaceBy => DisableShadowProcessing(replaceBy, _enableShadowCasting, _shadowProcessing), _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            _filterName = EditorGUILayout.TextField("Filter Name", _filterName);

            // Button to load
            if (GUILayout.Button($"{shadowCastingButtonLabel} shadow processing\nin {_selectionMode.ToPrint()} (filter by name)"))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(replaceBy => DisableShadowProcessing(replaceBy, _enableShadowCasting, _shadowProcessing, _filterName), _selectionMode);
            }

            EditorUtils.ShowHorizontalLine();

            // Button to load
            if (GUILayout.Button($"Revert {_selectionMode.ToPrint()}\nshadowing processing overrides."))
            {
                SelectionProcessingUtils.DecideOnHowToProcessSelection(replaceBy => RevertCastShadowsOverrides(replaceBy, _shadowProcessing), _selectionMode);
            }
        }

        private void RevertCastShadowsOverrides(SelectionMode selectionMode, ShadowProcessing shadowProcessing)
        {
            foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetCurrentlySelectedGameObjects(selectionMode))
            {
                if (PrefabUtility.IsPartOfPrefabInstance(selectionGameObject) == false)
                {
                    continue;
                }

                Renderer renderer = selectionGameObject.GetComponent<Renderer>();
                if (renderer == null)
                {
                    continue;
                }

                // Get the prefab asset corresponding to the instance
                GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(selectionGameObject);
                if (prefabAsset == null)
                {
                    continue;
                }

                Renderer assetRenderer = prefabAsset.GetComponent<Renderer>();
                if (assetRenderer == null)
                {
                    continue;
                }

                // Create SerializedObjects for both instance and asset renderers
                SerializedObject instanceRendererObject = new(renderer);

                // Revert the overrides
                if (shadowProcessing is ShadowProcessing.ShadowCasting or ShadowProcessing.Both)
                {
                    RevertOverrideIfExists(instanceRendererObject.FindProperty("m_CastShadows"));
                }

                if (shadowProcessing is ShadowProcessing.ReceiveShadows or ShadowProcessing.Both)
                {
                    RevertOverrideIfExists(instanceRendererObject.FindProperty("m_ReceiveShadows"));
                }
            }
        }

        private void RevertOverrideIfExists(SerializedProperty serializedProperty)
        {
            PrefabUtility.RevertPropertyOverride(serializedProperty, InteractionMode.UserAction);
        }

        private void DisableShadowProcessing(SelectionMode selectionMode, bool enableShadowCasting, ShadowProcessing shadowProcessing, string nameFilter = "")
        {
            foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetCurrentlySelectedGameObjects(selectionMode))
            {
                if (nameFilter != "" && selectionGameObject.name.Contains(nameFilter) == false)
                {
                    continue;
                }

                Renderer renderer = selectionGameObject.GetComponent<Renderer>();
                if (renderer == null)
                {
                    continue;
                }

                if (shadowProcessing is ShadowProcessing.ShadowCasting or ShadowProcessing.Both)
                {
                    renderer.shadowCastingMode = enableShadowCasting ? ShadowCastingMode.On : ShadowCastingMode.Off;
                }

                if (shadowProcessing is ShadowProcessing.ReceiveShadows or ShadowProcessing.Both)
                {
                    renderer.receiveShadows = enableShadowCasting;
                }
            }
        }

        private void ReplaceAllSelectedObjectsByPrefabFilterByComponents(SelectionMode selectionMode)
        {
            List<MonoBehaviour> targetPrefabComponents = _targetPrefab.GetComponents<MonoBehaviour>().ToList();

            List<GameObject> gameObjectsToReplace = new();
            foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetSelectedPlainGameObjects(selectionMode, _replacePrefabs))
            {
                // If the game object has the same components as the prefab, add it to the list
                // Order of components is not important
                if (targetPrefabComponents.All(component => selectionGameObject.GetComponent(component.GetType()) != null))
                {
                    gameObjectsToReplace.Add(selectionGameObject);
                }
            }

            ConvertListOfGameObjectsToPrefab(gameObjectsToReplace);
        }

        private void ReplaceAllSelectedObjectsByPrefab(SelectionMode selectionMode)
        {
            ConvertListOfGameObjectsToPrefab(SelectionProcessingUtils.GetSelectedPlainGameObjects(selectionMode, _replacePrefabs));
        }

        private void ReplaceAllSelectedObjectsByPrefabFilterByName(string filterName, SelectionMode selectionMode)
        {
            List<GameObject> gameObjectsToReplace = new();
            foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetSelectedPlainGameObjects(selectionMode, _replacePrefabs))
            {
                // If the mesh is the one we want to replace, add it to the list
                if (selectionGameObject.name.Contains(filterName))
                {
                    gameObjectsToReplace.Add(selectionGameObject);
                }
            }

            ConvertListOfGameObjectsToPrefab(gameObjectsToReplace);
        }

        private void ReplaceAllSelectedObjectsByPrefabFilterByMesh(SelectionMode selectionMode)
        {
            List<GameObject> gameObjectsToReplace = new();
            foreach (GameObject selectionGameObject in SelectionProcessingUtils.GetSelectedPlainGameObjects(selectionMode, _replacePrefabs))
            {
                MeshFilter[] childMeshFilters = selectionGameObject.GetComponentsInChildren<MeshFilter>();

                // If there is more than one mesh filter, we don't know if it's safe to replace
                if (childMeshFilters.Length != 1)
                {
                    continue;
                }

                // If the mesh is the one we want to replace, add it to the list
                if (childMeshFilters[0].sharedMesh == _filterMesh)
                {
                    gameObjectsToReplace.Add(selectionGameObject);
                }
            }

            ConvertListOfGameObjectsToPrefab(gameObjectsToReplace);
        }

        private void ConvertListOfGameObjectsToPrefab(IEnumerable<GameObject> gameObjectsToReplace)
        {
            foreach (GameObject gameObjectToReplace in gameObjectsToReplace)
            {
                if (gameObjectToReplace == null)
                {
                    Debug.LogWarning("An object was destroyed while replacing. This is most likely due to a parent being replaced first.");
                    continue;
                }

                Vector3 pos = gameObjectToReplace.transform.position;
                Quaternion rotation = gameObjectToReplace.transform.rotation;
                Vector3 scale = gameObjectToReplace.transform.localScale;

                bool isPrefabRoot = PrefabUtility.IsAnyPrefabInstanceRoot(gameObjectToReplace);
                bool isInsidePrefab = PrefabUtility.IsPartOfPrefabInstance(gameObjectToReplace);

                // Don't replace if the object is inside a prefab
                if (isPrefabRoot == false && isInsidePrefab)
                {
                    continue;
                }

                if (isPrefabRoot && _replacePrefabs == false)
                {
                    continue;
                }

                if (isPrefabRoot)
                {
                    PrefabUtility.ReplacePrefabAssetOfPrefabInstance(
                        gameObjectToReplace,
                        _targetPrefab,
                        new PrefabReplacingSettings { changeRootNameToAssetName = true },
                        InteractionMode.UserAction
                    );
                }
                else
                {
                    PrefabUtility.ConvertToPrefabInstance(
                        gameObjectToReplace,
                        _targetPrefab,
                        new ConvertToPrefabInstanceSettings { changeRootNameToAssetName = true },
                        InteractionMode.UserAction
                    );
                }

                gameObjectToReplace.transform.position = pos;
                gameObjectToReplace.transform.rotation = rotation;
                gameObjectToReplace.transform.localScale = scale;
            }
        }

        private enum ShadowProcessing
        {
            ShadowCasting,
            ReceiveShadows,
            Both
        }
    }
}