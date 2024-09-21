using System;
using System.Collections.Generic;
using Foxworks.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
	/// <summary>
	/// This class provides developers with a way to create custom preferences that will be displayed in the Preferences window.
	/// </summary>
	public static class FoxworksEditorPreferences
	{
		// Define a new section name for the User Settings
		private const string SettingsName = "Preferences/Foxworks";

		private const int SizeOfColumn = 200;

		private static IEnumerable<string> SearchKeywords => new HashSet<string>
		{
			"Foxworks"
		};
        
		public static BooleanEditorPref UseDoubleShiftSearchFunction => new("Foxworks.Editor.SearchEverywhere.UseDoubleShiftSearchFunction", true);
		public static FloatEditorPref DoubleShiftSearchFunctionClickTime => new("Foxworks.Editor.SearchEverywhere.DoubleShiftSearchFunctionClickTime", 0.2f);

		[SettingsProvider]
		public static SettingsProvider CreateMyCustomSettingsProvider()
		{
			// The path in the User Settings and the title displayed
			SettingsProvider provider = new(SettingsName, SettingsScope.User)
			{
				// Create the GUI for the new section:
				guiHandler = _ => { OnEditorGUI(); },

				// Populate the search keywords to enable smart search filtering and label highlighting
				keywords = SearchKeywords
			};

			return provider;
		}

		private static void OnEditorGUI()
		{
			ShowLabelWithControl(
				() => GUILayout.Label("Double Shift Search Function Click Time"),
				() => DoubleShiftSearchFunctionClickTime.Value = EditorGUILayout.FloatField(DoubleShiftSearchFunctionClickTime.Value)
			);

			ShowLabelWithControl(
				() => GUILayout.Label("Use Double Shift Search Function"),
				() => UseDoubleShiftSearchFunction.Value = EditorGUILayout.Toggle(UseDoubleShiftSearchFunction.Value)
			);
		}

		private static void ShowLabelWithControl(Action labelMethod, Action controlMethod)
		{
			WrapMethodInHorizontalLayoutWithWidth(SizeOfColumn * 2, () =>
			{
				WrapMethodInHorizontalLayoutWithWidth(SizeOfColumn, labelMethod);
				WrapMethodInHorizontalLayoutWithWidth(SizeOfColumn, controlMethod);
			});
		}

		private static void WrapMethodInHorizontalLayoutWithWidth(float width, Action method)
		{
			EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
			method();
			EditorGUILayout.EndHorizontal();
		}
	}
}