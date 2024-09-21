using System;
using System.IO;
using System.Threading.Tasks;
using Foxworks.Utils;
using UnityEngine;

namespace Foxworks.Persistence
{
	/// <summary>
	///     Manages saving and loading data to and from the persistent data path.
	/// </summary>
	public static class SaveManager
    {
        private const string SaveData = "SaveData";

        private static string PersistentDataPath { get; } = Application.persistentDataPath + "/SaveData/";

        /// <summary>
        ///     Saves data to the persistent data path.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void Save<T>(string dataId, T item)
        {
            string filePath = GetFilePathFromId(dataId);
            string data = SerializeData(item);
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        ///     Saves data to the persistent data path asynchronously.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task SaveAsync<T>(string dataId, T item)
        {
            string filePath = GetFilePathFromId(dataId);
            string data = SerializeData(item);
            await File.WriteAllTextAsync(filePath, data);
        }

        /// <summary>
        ///     Loads data from the persistent data path.
        /// </summary>
        /// <param name="dataId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string dataId)
        {
            string filePath = GetFilePathFromId(dataId);
            return File.Exists(filePath) == false ? default : LoadFromFile<T>(filePath);
        }

        /// <summary>
        ///     Loads data from the persistent data path asynchronously.
        /// </summary>
        /// <param name="dataId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(string dataId)
        {
            string filePath = GetFilePathFromId(dataId);
            if (File.Exists(filePath) == false)
            {
                return default;
            }

            return await LoadFromFileAsync<T>(filePath);
        }

        private static T LoadFromFile<T>(string filePath)
        {
            string data = File.ReadAllText(filePath);
            return DeserializeData<T>(data);
        }

        private static async Task<T> LoadFromFileAsync<T>(string filePath)
        {
            string data = await File.ReadAllTextAsync(filePath);
            return DeserializeData<T>(data);
        }

        private static string GetFilePathFromId(string dataId)
        {
            string fileName = $"{dataId}_{SaveData}.json";
            return Path.Combine(PersistentDataPath, fileName);
        }

        private static T DeserializeData<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single
            if (typeof(T).IsPrimitive)
            {
                T res = (T)Convert.ChangeType(data, typeof(T));
                return res;
            }

            T result = JsonUtility.FromJson<T>(data);
            return result;
        }

        private static string SerializeData(object obj)
        {
            PersistentDataPath.EnsureFolderExists();

            // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single
            string data = obj.GetType().IsPrimitive ? obj.ToString() : JsonUtility.ToJson(obj);
            return data;
        }
    }
}