using System;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Editor
{
	/// <summary>
	///     This class automatically closes the play mode when the editor is quitting.
	///     This is useful to avoid the editor to crash when quitting while the play mode is still on.
	/// </summary>
	[InitializeOnLoad]
    public class ExitPlayModeOnEditorQuit
    {
        static ExitPlayModeOnEditorQuit()
        {
            EditorApplication.wantsToQuit -= EditorWantsToQuit;
            EditorApplication.wantsToQuit += EditorWantsToQuit;
        }

        private static bool EditorWantsToQuit()
        {
            try
            {
                if (EditorApplication.isPlaying)
                {
                    Debug.Log("The editor wants to quit, but the play mode is still on. Stopping the play mode before quitting...");

                    EditorUtility.DisplayDialog("Quitting Unity...", "Stopping the play mode before quitting...", "OK");
                    EditorApplication.ExitPlaymode();
                    return false;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"An exception occurred while checking and stopping the play mode. Exception: {exception}");
            }

            return true;
        }
    }
}