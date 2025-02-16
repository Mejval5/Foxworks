using System;
using UnityEngine;

namespace Foxworks.Core
{
    /// <summary>
    ///     This class provides the singleton design pattern for Unity's MonoBehaviour scripts.
    ///     Never destroy this manually, let unity handle it on application quit or you won't be able to recreate it.
    /// </summary>
    public abstract class PersistentSingletonPrefab<T> : SingletonMonoBehavior<T> where T : SingletonMonoBehaviorInstance
    {
        // ReSharper disable once StaticMemberInGenericType
        private static bool isApplicationQuitting;
        
        /// <summary>
        /// Must be called in a static method to create the prefab instance.
        /// </summary>
        private static void CreatePrefabInstance()
        {
            if (instance != null)
            {
                UnityEngine.Debug.LogWarning($"Singleton prefab {typeof(T).Name} already instantiated.");
                return;
            }
            
            string singletonPrefabName = typeof(T).Name;
            T singletonPrefab = Resources.Load<T>(singletonPrefabName);
            if (singletonPrefab == null)
            {
                UnityEngine.Debug.LogError($"Error initializing the {singletonPrefabName} prefab. Could not locate the prefab asset.");
                return;
            }

            instance = Instantiate(singletonPrefab);
            DontDestroyOnLoad(instance.gameObject);
            instance.name = singletonPrefabName + " (Singleton)";
            UnityEngine.Debug.Log($"Singleton prefab {singletonPrefabName} instantiated.");
        }
        
        protected override void CustomInitializeSingleton()
        {
            base.CustomInitializeSingleton();
            
            DontDestroyOnLoad(gameObject);
        }

        protected override void CustomAwake()
        {
            base.CustomAwake();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            // If we are entering edit mode, reset the application quitting flag
            if (state is UnityEditor.PlayModeStateChange.EnteredEditMode or UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                isApplicationQuitting = false;
            }
        }
#endif

        protected override void CustomOnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                isApplicationQuitting = true;
            }
            
            base.CustomOnDestroy();
        }

        public static new T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                
                if (isApplicationQuitting)
                {
                    return null;
                }
                
                // Create the prefab instance
                CreatePrefabInstance();
                return instance;
            }
        }
    }

    /// <summary>
    ///     This class extends the MonoBehaviour class and adds a protected `InitializeSingleton` method to be called when initializing the entity.
    /// </summary>
    public class SingletonMonoBehaviorInstance : MonoBehaviour
    {
        internal bool IsInitialized { get; private set; }

        /// <summary>
        ///     Implement this method for initialization. Do not call it yourself.
        /// </summary>
        internal void InitializeSingleton()
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Instance already initialized.");
            }

            CustomInitializeSingleton();
            IsInitialized = true;
        }

        protected virtual void CustomInitializeSingleton()
        {
        }
    }
    
    /// <summary>
    /// To be used as a base class for all singletons.
    /// </summary>
    public abstract class SingletonMonoBehavior<T> : SingletonMonoBehaviorInstance where T : SingletonMonoBehaviorInstance
    {
        protected static T instance;

        public static bool HasInstance => instance != null;

        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                // Find existing instance
                instance = FindFirstObjectByType(typeof(T)) as T;

                if (instance == null)
                {
                    // GameObject singletonObject = new();
                    // instance = singletonObject.AddComponent<T>();
                    // singletonObject.name = typeof(T) + " (Singleton)";
                    return null;
                }

                if (!instance.IsInitialized)
                {
                    instance.InitializeSingleton();
                }

                return instance;
            }
        }

        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                InitializeSingleton();
                CustomAwake();
            }
            else if (instance == this)
            {
                CustomAwake();
            }
            else if (instance != this)
            {
                Debug.LogWarningFormat($"A singleton [{typeof(T).Name}] already exists!");
                DestroyImmediate(this);
            }
        }

        protected virtual void CustomAwake() {}

        protected void OnDestroy()
        {
            CustomOnDestroy();
            
            if (instance == this)
            {
                instance = null;
            }
        }

        protected virtual void CustomOnDestroy() {}
    }

    /// <summary>
    ///     This class extends the ScriptableObject to allow auto loading it into memory.
    ///     Useful for storing data that needs to be accessed from multiple scenes.
    /// </summary>
    public class ScriptableObjectSingleton<TObject> : ScriptableObject where TObject : ScriptableObject
    {
        private static TObject instance;

        public static TObject Instance
        {
            get
            {
                if (instance == null) CreateOrLoadInstance();
                return instance;
            }
        }

        private static void CreateOrLoadInstance()
        {
            string singletonName = typeof(TObject).Name;
            instance = Resources.Load<TObject>(singletonName);

#if UNITY_EDITOR
            if (instance != null) return;
            instance = CreateInstance<TObject>();
            UnityEditor.AssetDatabase.CreateAsset(instance, $"Assets/Resources/{singletonName}.asset");
#endif
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                return;
            }

            if (instance == null || instance == this)
            {
                return;
            }

            Debug.LogError($"An instance of {typeof(TObject)} already exist. Ensure only one scriptable object is present."
                           + $"To find it use: `t: {typeof(TObject)}` in the search bar.");
            DestroyImmediate(this, true);
#endif
        }
    }
}
    
