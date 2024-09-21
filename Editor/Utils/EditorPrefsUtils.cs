using UnityEditor;

namespace Foxworks.Editor.Utils
{
	public class BooleanEditorPref : KeyBasedPersistantStorage<bool>
	{
		public BooleanEditorPref(string key, bool defaultValue = default) : base(key, defaultValue)
		{
			_get = EditorPrefs.GetBool;
			_set = EditorPrefs.SetBool;
		}
	}

	public class FloatEditorPref : KeyBasedPersistantStorage<float>
	{
		public FloatEditorPref(string key, float defaultValue = default) : base(key, defaultValue)
		{
			_get = EditorPrefs.GetFloat;
			_set = EditorPrefs.SetFloat;
		}
	}

	public class IntegerEditorPref : KeyBasedPersistantStorage<int>
	{
		public IntegerEditorPref(string key, int defaultValue = default) : base(key, defaultValue)
		{
			_get = EditorPrefs.GetInt;
			_set = EditorPrefs.SetInt;
		}
	}

	public class StringEditorPref : KeyBasedPersistantStorage<string>
	{
		public StringEditorPref(string key, string defaultValue = default) : base(key, defaultValue)
		{
			_get = EditorPrefs.GetString;
			_set = EditorPrefs.SetString;
		}
	}
}