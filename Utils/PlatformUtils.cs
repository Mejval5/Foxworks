using UnityEngine;

namespace Foxworks.Utils
{
    public class PlatformUtils
    {
        /// <summary>
        ///     Returns the target platform of the build.
        /// </summary>
        public static RuntimePlatform TargetPlatform
        {
            get
            {
#if UNITY_ANDROID
                return RuntimePlatform.Android;
#elif UNITY_STANDALONE_LINUX
                return RuntimePlatform.LinuxPlayer;
#elif UNITY_WEBGL
                return RuntimePlatform.WebGLPlayer;
#elif UNITY_IOS
                return RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
                return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
                return RuntimePlatform.WindowsPlayer;
#endif
            }
        }

        public static bool IsTargetingAndroid => TargetPlatform == RuntimePlatform.Android;
        public static bool IsTargetingWindows => TargetPlatform == RuntimePlatform.WindowsPlayer;
        public static bool IsTargetingWebGL => TargetPlatform == RuntimePlatform.WebGLPlayer;
    }
}