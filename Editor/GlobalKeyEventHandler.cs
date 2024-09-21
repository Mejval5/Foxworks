using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
    /// <summary>
    /// Thanks to https://forum.unity.com/threads/4-6-editorapplication-modifierkeyschanged-how-to-find-out-which-key-was-pressed.357367/#post-2705846
    /// </summary>
    [InitializeOnLoad]
    public static class GlobalKeyEventHandler
    {
        public static bool RegistrationSucceeded;

        static GlobalKeyEventHandler()
        {
            RegistrationSucceeded = false;
            string msg = "";
            try
            {
                FieldInfo info = typeof(EditorApplication).GetField(
                    "globalEventHandler",
                    BindingFlags.Static | BindingFlags.NonPublic
                );
                if (info != null)
                {
                    EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue(null);

                    value -= onKeyPressed;
                    value += onKeyPressed;

                    info.SetValue(null, value);

                    RegistrationSucceeded = true;
                }
                else
                {
                    msg = "globalEventHandler not found";
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            finally
            {
                if (!RegistrationSucceeded)
                {
                    Debug.LogWarning("GlobalKeyEventHandler: error while registering for globalEventHandler: " + msg);
                }
            }
        }

        public static event Action<Event> OnKeyEvent;

        private static void onKeyPressed()
        {
            OnKeyEvent?.Invoke(Event.current);
        }
    }
}