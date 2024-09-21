using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor.Utils
{
    public class EditorUtils
    {
        /// <summary>
        /// Easy way to show a horizontal line in the editor.
        /// </summary>
        public static void ShowHorizontalLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }
}