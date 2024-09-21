using System;

namespace Foxworks.Editor
{
	/// <summary>
	/// Inherit this to create a persistent storage that is based on a key.
	/// This simplifies usage of PlayerPrefs and EditorPrefs.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class KeyBasedPersistantStorage<T>
	{
		private readonly string _key;
		private readonly T _defaultValue;

		protected KeyBasedPersistantStorage(string key, T defaultValue = default)
		{
			_key = key;
			_defaultValue = defaultValue;
		}

		public T Value
		{
			get => _get(_key, _defaultValue);
			set => _set(_key, value);
		}

		protected Func<string, T, T> _get = (_, _) => throw new NotImplementedException();
		protected Action<string, T> _set = (_, _) => throw new NotImplementedException();
	}
}