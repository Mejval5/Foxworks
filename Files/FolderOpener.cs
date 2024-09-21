using System.Diagnostics;

namespace Foxworks.Files
{
    public static class FolderOpener
    {
        public static void OpenFolder(string folderPath)
        {
            // Check if the folder path exists to avoid exceptions
            if (System.IO.Directory.Exists(folderPath))
            {
                // Start Explorer.exe with the folder path directly
                Process.Start("explorer.exe", $"\"{folderPath}\"");
            }
            else
            {
                UnityEngine.Debug.LogError("Folder path does not exist: " + folderPath);
            }
        }
    }
}