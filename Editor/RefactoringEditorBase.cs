using System.Collections.Generic;
using System.Linq;
using Editor.Fox.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Fox
{
	using SelectionMode = SelectionProcessingUtils.SelectionMode;
	
	public abstract class RefactoringEditorBase : EditorWindow
	{
		protected const float WindowWidth = 500f;
		protected const float WindowHeight = 400f;
		
		[SerializeField] private List<GameObject> _selectedObjects;

		protected bool _overrideSelection;
		protected bool _replacePrefabs;
		private SerializedProperty _selectedObjectsProperty;
		protected SelectionMode _selectionMode;
		protected SerializedObject _serializedObject;

		private void OnEnable()
		{
			_serializedObject = new SerializedObject(this);
			_selectedObjects = new List<GameObject>();
			_selectedObjectsProperty = _serializedObject.FindProperty("_selectedObjects");
		}

		protected void OnGUI()
		{
			_selectionMode = (SelectionMode)EditorGUILayout.EnumPopup("Replace mode", _selectionMode);

			GUILayout.BeginHorizontal();
			_overrideSelection = EditorGUILayout.Toggle("Override selection", _overrideSelection);
			if (_overrideSelection)
			{
				_selectedObjects = _selectedObjects.Where(obj => obj != null).ToList();
				//render the list of objects
				_serializedObject.Update();
				EditorGUILayout.PropertyField(_selectedObjectsProperty, new GUIContent("Overriden Selection"), true);
				_serializedObject.ApplyModifiedProperties();
			}
			GUILayout.EndHorizontal();

			_replacePrefabs = EditorGUILayout.Toggle("Can replace Prefabs", _replacePrefabs);

			EditorUtils.ShowHorizontalLine();

			DrawCustomGUI();

			EditorUtils.ShowHorizontalLine();

			// show info box
			EditorGUILayout.HelpBox("This tool's changes can be undone, CTRL+Z.\n"
			                        + "Still, don't forget to check in your work often.",
				MessageType.Info);
		}

		protected abstract void DrawCustomGUI();
	}
}