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
        
        /// <summary>
        /// Loads a texture from a file path (JPG or PNG).
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Texture2D LoadTextureFromFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new (2, 2); // Create a new texture (size will be overwritten by LoadImage)
            return texture.LoadImage(fileData) ? texture : null;
        }
    }
}