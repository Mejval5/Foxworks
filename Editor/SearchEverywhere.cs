using System;
using Foxworks.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
    /// <summary>
    /// This class allows to open the search all window by pressing shift twice.
    /// </summary>
    [InitializeOnLoad]
    public static class SearchEverywhere
    {
        private static DateTime _lastShiftPressDownTime = DateTime.MinValue;
        private static DateTime _lastShiftPressUpTime = DateTime.MinValue;

        static SearchEverywhere()
        {
            GlobalKeyEventHandler.OnKeyEvent -= OnKeyEvent;
            GlobalKeyEventHandler.OnKeyEvent += OnKeyEvent;
        }

        private static void OnKeyEvent(Event current)
        {
            if (current == null)
            {
                return;
            }
            
            if (FoxworksEditorPreferences.UseDoubleShiftSearchFunction.Value == false)
            {
                return;
            }

            if (current.keyCode is not KeyCode.LeftShift)
            {
                _lastShiftPressDownTime = DateTime.MinValue;
                _lastShiftPressUpTime = DateTime.MinValue;
                return;
            }

            if (current.type != EventType.KeyUp)
            {
                _lastShiftPressUpTime = DateTime.Now;
                return;
            }

            if (current.type != EventType.KeyDown)
            {
                TryOpen();
                _lastShiftPressDownTime = DateTime.Now;
            }
        }

        private static void TryOpen()
        {
            TimeSpan downPressTimeSpan = DateTime.Now - _lastShiftPressDownTime;
            TimeSpan upPressTimeSpan = DateTime.Now - _lastShiftPressUpTime;
            
            TimeSpan delay = TimeSpan.FromSeconds(FoxworksEditorPreferences.DoubleShiftSearchFunctionClickTime.Value);

            if (downPressTimeSpan < delay && upPressTimeSpan < delay)
            {
                // Open search all window
                EditorApplication.ExecuteMenuItem("Edit/Search All...");
            }
        }
    }
}