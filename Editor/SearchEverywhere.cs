using System;
using UnityEditor;
using UnityEngine;

namespace Editor.Fox
{
    [InitializeOnLoad]
    public static class SearchEverywhere
    {
        private static TimeSpan Delay = TimeSpan.FromSeconds(0.2f);
        
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
            
            if (downPressTimeSpan < Delay && upPressTimeSpan < Delay)
            {
                // Open search all window
                EditorApplication.ExecuteMenuItem("Edit/Search All...");
            }
        }
    }
}


