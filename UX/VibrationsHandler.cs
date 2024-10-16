﻿using UnityEngine;

namespace Foxworks.UX
{
    public static class VibrationsHandler
    {
        /// <summary>
        ///     Vibrates the device.
        ///     Works on Android and iOS.
        /// </summary>
        public static void Vibrate()
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
    }
}