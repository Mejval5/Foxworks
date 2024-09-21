using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Foxworks.Editor
{
    /// <summary>
    /// Adds extra buttons to the toolbar.
    /// </summary>
    [InitializeOnLoad]
    public class ToolbarButtons
    {
        static ToolbarButtons()
        {
            ToolbarExtender.Editor.ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.Space(3);
            if (GUILayout.Button(new GUIContent("Refresh", "Refreshes the asset database manually.")))
            {
                AssetDatabase.Refresh();
            }

            GUILayout.Space(3);
            if (GUILayout.Button(new GUIContent("VSCode", "Opens VS Code on top level of the project.")))
            {
                OpenFolderInVSCodeInsiders();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("RC", "Refreshes the asset database and recompiles all scripts manually.")))
            {
                AssetDatabase.Refresh();
                CompilationPipeline.RequestScriptCompilation();
            }
        }

        public static void OpenFolderInVSCodeInsiders()
        {
            // The path to your top-level folder (replace with your desired folder path)
            string folderPath = Path.Combine(Application.dataPath, ".."); // This opens the project root folder

            // Find the path to code-insiders in the system PATH
            string codeInsidersPath = FindExecutableInPath("code-insiders");
            codeInsidersPath += ".cmd";

            if (string.IsNullOrEmpty(codeInsidersPath))
            {
                Debug.LogError("VS Code Insiders not found in the system PATH.");
                return;
            }

            // Create the process start information
            ProcessStartInfo processStartInfo = new()
            {
                FileName = codeInsidersPath, // Path to the code-insiders executable
                Arguments = $"\"{folderPath}\"", // The folder to open
                UseShellExecute = false, // Do not use the shell to execute
                RedirectStandardOutput = false, // No need to redirect output
                RedirectStandardError = false, // No need to redirect errors
                CreateNoWindow = true // Prevents a command prompt window from appearing
            };

            // On Windows, use `CreateNoWindow` and set `UseShellExecute = false` to detach the process
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardInput = true; // Required for detaching
            }

            // Start the process
            Process process = new()
            {
                StartInfo = processStartInfo
            };
            process.Start();
        }

        private static string FindExecutableInPath(string executableName)
        {
            // Get the PATH environment variable
            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathEnv))
            {
                return null;
            }

            // Split the PATH into individual directories
            string[] paths = pathEnv.Split(Path.PathSeparator);

            // Check each directory for the executable
            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, executableName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }

                // On Windows, check for the .exe extension
                if (Application.platform == RuntimePlatform.WindowsEditor)
                {
                    fullPath = Path.Combine(path, executableName + ".exe");
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }

            // Return null if the executable was not found
            return null;
        }
    }
}