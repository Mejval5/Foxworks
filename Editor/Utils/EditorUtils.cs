using UnityEditor;
using UnityEngine;

namespace Editor.Fox.Utils
{
    public class EditorUtils
    {
        public static void ShowHorizontalLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }
}