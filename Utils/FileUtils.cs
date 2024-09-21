using System;
using System.IO;
using UnityEngine;

namespace Foxworks.Utils
{
    public static class FileUtils
    {
        /// <summary>
        ///     Ensures that a folder exists at the specified path.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
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