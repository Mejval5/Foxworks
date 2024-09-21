using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Editor.Fox
{
	public static class SelectionProcessingUtils
    {
        public enum SelectionMode
        {
            OnlySelectedObjects,
            WholeScene,
            SelectedPrefabsChildren
        }
        
        public static string ToPrint(this SelectionMode selectionMode) => selectionMode switch
        {
            SelectionMode.OnlySelectedObjects => "selected objects",
            SelectionMode.WholeScene => "whole scene",
            SelectionMode.SelectedPrefabsChildren => "selected prefabs' children",
            _ => "selected objects"
        };
        
        private static IEnumerable<GameObject> GetAllSelectedObjects(UnityEditor.SelectionMode selectionMode = UnityEditor.SelectionMode.Unfiltered)
        {
            Object[] selectionObjects = Selection.GetFiltered<Object>(selectionMode);

            return selectionObjects.Where(selectionObject => selectionObject != null).OfType<GameObject>().ToList();
        }

        public static IEnumerable<GameObject> GetCurrentlySelectedGameObjects(SelectionMode selectionMode)
        {
            // return based on the current selection
            switch (selectionMode)
            {
                case SelectionMode.OnlySelectedObjects:
                    return GetAllSelectedObjects();

                case SelectionMode.WholeScene:
                    return GetAllCurrentStageObjects();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static IEnumerable<GameObject> GetSelectedPlainGameObjects(SelectionMode selectionMode, bool replacePrefabs = false)
        {
            IEnumerable<GameObject> selectionObjects = GetCurrentlySelectedGameObjects(selectionMode);
            if (replacePrefabs)
            {
                return selectionObjects;
            }

            return selectionObjects.Where(selectionGameObject => !PrefabUtility.IsPartOfPrefabInstance(selectionGameObject)).ToList();
        }

        private static IEnumerable<GameObject> GetAllCurrentStageObjects()
        {
            List<GameObject> parents = new ();
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                parents.Add(PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot);
            }
            else
            {
                // get top level items in scene hierarchy
                parents.AddRange(SceneManager.GetActiveScene().GetRootGameObjects());
            }

            return parents.SelectMany(parent => parent.transform.GetComponentsInChildren<Transform>()).Select(trans => trans.gameObject);
        }
        
        public static void DecideOnHowToProcessSelection(Action<SelectionMode> action, SelectionMode selectionMode)
        {
        	if (selectionMode is SelectionMode.SelectedPrefabsChildren)
        	{
        		ProcessSelectedPrefabChildrenGameObjects(action);
        	}
        	else
        	{
        		action.Invoke(selectionMode);
        	}
        }

        private static void ProcessSelectedPrefabChildrenGameObjects(Action<SelectionMode> action)
        {
        	IEnumerable<GameObject> selectedPrefabs = GetAllSelectedObjects(UnityEditor.SelectionMode.Assets);
        	int allProcessedItems = 0;
        	foreach (GameObject selectedPrefab in selectedPrefabs)
        	{
        		ProcessSinglePrefabChildrenGameObjects(selectedPrefab, action);
        		allProcessedItems += 1;
        	}

        	// Refresh the asset database to reflect changes
        	AssetDatabase.Refresh();

        	Debug.Log("All processed items: " + allProcessedItems);
        }

        private static void ProcessSinglePrefabChildrenGameObjects(GameObject selectedPrefab, Action<SelectionMode> action)
        {
        	if (PrefabUtility.GetPrefabAssetType(selectedPrefab) == PrefabAssetType.NotAPrefab)
        	{
        		return;
        	}

        	string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);

        	// If extension is not prefab don't do anything
        	if (prefabPath.EndsWith(".prefab") == false)
        	{
        		return;
        	}

        	PrefabStage prefabStage = PrefabStageUtility.OpenPrefab(prefabPath);

        	action.Invoke(SelectionMode.WholeScene);

        	GameObject root = prefabStage.prefabContentsRoot;
        	PrefabUtility.SaveAsPrefabAsset(root, prefabPath);

        	// Close the prefab stage
        	PrefabStageUtility.GetCurrentPrefabStage().ClearDirtiness();
        }
    }
}