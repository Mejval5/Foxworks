using System;
using System.IO;
using Fox.Utils;
using UnityEngine;

namespace Fox.Persistence
{
	public static class SaveManager
	{
		private const string SaveData = "SaveData";
		
		private static string PersistentDataPath { get; } = Application.persistentDataPath + "/SaveData/";

		public static void Save<T>(string dataId, T item)
		{
			string filePath = GetFilePathFromId(dataId);
			string data = SerializeData(item);
			File.WriteAllText(filePath, data);
		}

		public static T Load<T>(string dataId)
		{
			string filePath = GetFilePathFromId(dataId);
			return File.Exists(filePath) == false ? default : LoadFromFile<T>(filePath);
		}

		private static T LoadFromFile<T>(string filePath)
		{
			string data = File.ReadAllText(filePath);
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
				return default(T);

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