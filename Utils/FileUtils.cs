using System;
using System.IO;
using UnityEngine;

namespace Fox.Utils
{
    public static class FileUtils
    {
        public static bool EnsureFolderExists(this string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath) == false)
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to create a folder at path: {folderPath}. Exception: {exception}.");
                return false;
            }

            return true;
        }
    }
}