namespace Foxworks.Core
{
    public class LazySingleton<T> where T : new()
    {
        private static T instance;
        
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
                    UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
                }

                return instance;
            }
        }
        
        
#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            // If we are entering edit mode, reset the application quitting flag
            if (state is UnityEditor.PlayModeStateChange.EnteredEditMode or UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                instance = default;
            }
        }
#endif
    }
}
