using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Foxworks.Utils;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Foxworks.Persistence
{
    /// <summary>
    ///     Manages saving and loading data to and from the persistent data path.
    /// </summary>
    public static class SaveManager
    {
        private static bool DebugMode { get; } = false;
        
        private const string SaveData = "SaveData";
        private const string SaveDataExtension = ".json";
        private static string[] ByteDataExtensions { get; } = {".zip", ".png", ".bytes"};

        private static string PersistentDataPath { get; } = Application.persistentDataPath + "/SaveData/";
        
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Semaphores = new();
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> SaveCancelTokens = new ();

        static SaveManager()
        {
            PersistentDataPath.EnsureFolderExists();
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= ResetInEditor;
            EditorApplication.playModeStateChanged += ResetInEditor;
#endif
        }

#if UNITY_EDITOR
        private static void ResetInEditor(PlayModeStateChange state)
        {
            if (state is not (PlayModeStateChange.EnteredPlayMode or PlayModeStateChange.EnteredEditMode))
            {
                return;
            }
            
            foreach (KeyValuePair<string, CancellationTokenSource> tokens in SaveCancelTokens)
            {
                tokens.Value.Cancel();
                tokens.Value.Dispose();
            }
            
            SaveCancelTokens.Clear();

            foreach (KeyValuePair<string, SemaphoreSlim> semaphore in Semaphores)
            {
                semaphore.Value.Dispose();
            }
            
            Semaphores.Clear();
        }
#endif

        private static void Log(string message)
        {
            if (DebugMode)
            {
                Debug.Log(message);
            }
        }
        
        /// <summary>
        ///     Saves data to the persistent data path.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="item"></param>
        /// <param name="extension"></param>
        /// <typeparam name="T"></typeparam>
        public static void Save<T>(string dataId, T item, string extension = SaveDataExtension)
        {
            Log($"Saving data to {dataId}");
            
            string filePath = GetFilePathFromId(dataId, extension);

            if (item is byte[] bytes)
            {
                File.WriteAllBytes(filePath, bytes);
                return;
            }
            
            string data = SerializeData(item);
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        ///     Saves data to the persistent data path asynchronously.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="item"></param>
        /// <param name="extension"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task SaveAsync<T>(string dataId, T item, string extension = SaveDataExtension)
        {
            Log($"Saving data to {dataId}");
            
            SemaphoreSlim semaphore = Semaphores.GetOrAdd(dataId, _ => new SemaphoreSlim(1, 1));
    
            // Cancel any existing save operation for this dataId
            if (SaveCancelTokens.TryGetValue(dataId, out CancellationTokenSource cts))
            {
                cts.Cancel(); // Cancel the previous save task
                cts.Dispose();
            }

            // Create a new cancellation token for the current save
            cts = new CancellationTokenSource();
            SaveCancelTokens[dataId] = cts;

            await semaphore.WaitAsync();
            
            try
            {
                string filePath = GetFilePathFromId(dataId, extension);

                if (item is byte[] bytes)
                {
                    await File.WriteAllBytesAsync(filePath, bytes);
                }
                else
                {
                    string data = SerializeData(item);
                    await File.WriteAllTextAsync(filePath, data);
                }
            }
            catch (OperationCanceledException)
            {
                // If operation was cancelled, just exit
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        ///     Loads data from the persistent data path.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="extension"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string dataId, string extension = SaveDataExtension)
        {
            Log($"Loading data from {dataId}");
            
            string filePath = GetFilePathFromId(dataId, extension);
            return File.Exists(filePath) == false ? default : LoadFromFile<T>(filePath);
        }

        /// <summary>
        ///     Loads data from the persistent data path asynchronously.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="extension"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> LoadAsync<T>(string dataId, string extension = SaveDataExtension)
        {
            Log($"Loading data from {dataId}");
            
            SemaphoreSlim semaphore = Semaphores.GetOrAdd(dataId, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            
            try
            {
                string filePath = GetFilePathFromId(dataId, extension);
                return File.Exists(filePath) == false ? default : await LoadFromFileAsync<T>(filePath);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        ///     Checks if the data exists in the persistent data path.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool Exists(string dataId, string extension = SaveDataExtension)
        {
            string filePath = GetFilePathFromId(dataId, extension);
            return File.Exists(filePath);
        }

        private static T LoadFromFile<T>(string filePath)
        {
            if (ByteDataExtensions.Contains(Path.GetExtension(filePath)))
            {
                byte[] data = File.ReadAllBytes(filePath);
        
                // Check if T is byte[], return the data if it is
                if (typeof(T) == typeof(byte[]))
                {
                    return (T)(object)data;
                }
                
                throw new InvalidCastException($"Cannot cast byte[] to {typeof(T)}. Make sure the file format matches the expected type.");
            }
            else
            {
                string data = File.ReadAllText(filePath);
                return DeserializeData<T>(data);
            }
        }

        private static async Task<T> LoadFromFileAsync<T>(string filePath)
        {
            if (ByteDataExtensions.Contains(Path.GetExtension(filePath)))
            {
                byte[] data = await File.ReadAllBytesAsync(filePath);
                
                // Check if T is byte[], return the data if it is
                if (typeof(T) == typeof(byte[]))
                {
                    return (T)(object)data;
                }
                
                // Handle error case where T is not byte[]
                throw new InvalidCastException($"Cannot cast byte[] to {typeof(T)}. Make sure the file format matches the expected type.");
            }
            else
            {
                string data = await File.ReadAllTextAsync(filePath);
                return DeserializeData<T>(data);
            }
        }

        private static string GetFilePathFromId(string dataId, string extension)
        {
            string fileName = $"{dataId}_{SaveData}.{extension}";
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
            // Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single
            string data = obj.GetType().IsPrimitive ? obj.ToString() : JsonUtility.ToJson(obj);
            return data;
        }
    }
}