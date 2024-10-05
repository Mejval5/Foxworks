using System;
using UnityEngine;

namespace Foxworks.Utils
{
    public enum EasingType
    {
        Linear,
        BounceIn,
        BounceOut,
        BounceInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BackIn,
        BackOut,
        BackInOut,
        SineIn,
        SineOut,
        SineInOut,
        QuintIn,
        QuintOut,
        QuintInOut,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuadIn,
        QuadOut,
        QuadInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        CircIn,
        CircOut,
        CircInOut,
        CubicIn,
        CubicOut,
        CubicInOut
    }

    public static class EasingUtils
    {
        /// <summary>
        ///     Returns the easing function based on the easing type.
        /// </summary>
        /// <param name="easingType"></param>
        public static Func<float, float> GetEasingFunction(EasingType easingType)
        {
            return easingType switch
            {
                EasingType.Linear => Linear,
                EasingType.BounceIn => BounceIn,
                EasingType.BounceOut => BounceOut,
                EasingType.BounceInOut => BounceInOut,
                EasingType.ElasticIn => ElasticIn,
                EasingType.ElasticOut => ElasticOut,
                EasingType.ElasticInOut => ElasticInOut,
                EasingType.BackIn => BackIn,
                EasingType.BackOut => BackOut,
                EasingType.BackInOut => BackInOut,
                EasingType.SineIn => SineIn,
                EasingType.SineOut => SineOut,
                EasingType.SineInOut => SineInOut,
                EasingType.QuintIn => QuintIn,
                EasingType.QuintOut => QuintOut,
                EasingType.QuintInOut => QuintInOut,
                EasingType.QuartIn => QuartIn,
                EasingType.QuartOut => QuartOut,
                EasingType.QuartInOut => QuartInOut,
                EasingType.QuadIn => QuadIn,
                EasingType.QuadOut => QuadOut,
                EasingType.QuadInOut => QuadInOut,
                EasingType.ExpoIn => ExpoIn,
                EasingType.ExpoOut => ExpoOut,
                EasingType.ExpoInOut => ExpoInOut,
                EasingType.CircIn => CircIn,
                EasingType.CircOut => CircOut,
                EasingType.CircInOut => CircInOut,
                EasingType.CubicIn => CubicIn,
                EasingType.CubicOut => CubicOut,
                EasingType.CubicInOut => CubicInOut,
                _ => throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null)
            };
        }

        /// <summary>
        ///     Bounce easing in function.
        /// </summary>
        public static float BounceIn(float x)
        {
            return 1 - BounceOut(1 - x);
        }

        /// <summary>
        ///     Bounce easing out function.
        /// </summary>
        public static float BounceOut(float x)
        {
            if (x < 1 / 2.75)
            {
                return 7.5625f * x * x;
            }

            if (x < 2 / 2.75)
            {
                return 7.5625f * (x -= 1.5f / 2.75f) * x + 0.75f;
            }

            if (x < 2.5 / 2.75)
            {
                return 7.5625f * (x -= 2.25f / 2.75f) * x + 0.9375f;
            }

            return 7.5625f * (x -= 2.625f / 2.75f) * x + 0.984375f;
        }

        /// <summary>
        ///     Bounce easing in-out function.
        /// </summary>
        public static float BounceInOut(float x)
        {
            return x < 0.5 ? (1 - BounceOut(1 - 2 * x)) / 2 : (1 + BounceOut(2 * x - 1)) / 2;
        }

        /// <summary>
        ///     Elastic easing in function.
        /// </summary>
        public static float ElasticIn(float x)
        {
            return Mathf.Sin(13 * Mathf.PI / 2 * x) * Mathf.Pow(2, 10 * (x - 1));
        }

        /// <summary>
        ///     Elastic easing out function.
        /// </summary>
        public static float ElasticOut(float x)
        {
            return Mathf.Sin(-13 * Mathf.PI / 2 * (x + 1)) * Mathf.Pow(2, -10 * x) + 1;
        }

        /// <summary>
        ///     Elastic easing in-out function.
        /// </summary>
        public static float ElasticInOut(float x)
        {
            return x < 0.5 ? (1 - ElasticOut(1 - 2 * x)) / 2 : (1 + ElasticOut(2 * x - 1)) / 2;
        }

        /// <summary>
        ///     Back easing in function.
        /// </summary>
        public static float BackIn(float x)
        {
            return x * x * x - x * Mathf.Sin(x * Mathf.PI);
        }

        /// <summary>
        ///     Back easing out function.
        /// </summary>
        public static float BackOut(float x)
        {
            return 1 - BackIn(1 - x);
        }

        /// <summary>
        ///     Back easing in-out function.
        /// </summary>
        public static float BackInOut(float x)
        {
            return x < 0.5 ? (1 - BackOut(1 - 2 * x)) / 2 : (1 + BackOut(2 * x - 1)) / 2;
        }

        /// <summary>
        ///     Sine easing in function.
        /// </summary>
        public static float SineIn(float x)
        {
            return 1 - Mathf.Cos(x * Mathf.PI / 2);
        }

        /// <summary>
        ///     Sine easing out function.
        /// </summary>
        public static float SineOut(float x)
        {
            return Mathf.Sin(x * Mathf.PI / 2);
        }

        /// <summary>
        ///     Sine easing in-out function.
        /// </summary>
        public static float SineInOut(float x)
        {
            return (1 - Mathf.Cos(Mathf.PI * x)) / 2;
        }

        /// <summary>
        ///     Quint easing in function.
        /// </summary>
        public static float QuintIn(float x)
        {
            return x * x * x * x * x;
        }

        /// <summary>
        ///     Quint easing out function.
        /// </summary>
        public static float QuintOut(float x)
        {
            return 1 - Mathf.Pow(1 - x, 5);
        }

        /// <summary>
        ///     Quint easing in-out function.
        /// </summary>
        public static float QuintInOut(float x)
        {
            return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
        }

        /// <summary>
        ///     Quart easing in function.
        /// </summary>
        public static float QuartIn(float x)
        {
            return x * x * x * x;
        }

        /// <summary>
        ///     Quart easing out function.
        /// </summary>
        public static float QuartOut(float x)
        {
            return 1 - Mathf.Pow(1 - x, 4);
        }

        /// <summary>
        ///     Quart easing in-out function.
        /// </summary>
        public static float QuartInOut(float x)
        {
            return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
        }

        /// <summary>
        ///     Quad easing in function.
        /// </summary>
        public static float QuadIn(float x)
        {
            return x * x;
        }

        /// <summary>
        ///     Quad easing out function.
        /// </summary>
        public static float QuadOut(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }

        /// <summary>
        ///     Quad easing in-out function.
        /// </summary>
        public static float QuadInOut(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }

        /// <summary>
        ///     Expo easing in function.
        /// </summary>
        public static float ExpoIn(float x)
        {
            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
        }

        /// <summary>
        ///     Expo easing out function.
        /// </summary>
        public static float ExpoOut(float x)
        {
            return Mathf.Approximately(x, 1) ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }

        /// <summary>
        ///     Expo easing in-out function.
        /// </summary>
        public static float ExpoInOut(float x)
        {
            return x == 0 ? 0 : Mathf.Approximately(x, 1) ? 1 : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }

        /// <summary>
        ///     Circular easing in function.
        /// </summary>
        public static float CircIn(float x)
        {
            return 1 - Mathf.Sqrt(1 - x * x);
        }

        /// <summary>
        ///     Circular easing out function.
        /// </summary>
        public static float CircOut(float x)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
        }

        /// <summary>
        ///     Circular easing in-out function.
        /// </summary>
        public static float CircInOut(float x)
        {
            return x < 0.5 ? (1 - Mathf.Sqrt(1 - 4 * x * x)) / 2 : (Mathf.Sqrt(1 - 4 * (x - 1) * (x - 1)) + 1) / 2;
        }

        /// <summary>
        ///     Linear easing function.
        /// </summary>
        public static float Linear(float x)
        {
            return x;
        }

        /// <summary>
        ///     Simple cubic easing in function.
        /// </summary>
        public static float CubicInOut(float x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }

        /// <summary>
        ///     Simple cubic easing out function.
        /// </summary>
        public static float CubicOut(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }

        /// <summary>
        ///     Simple cubic easing in function.
        /// </summary>
        public static float CubicIn(float x)
        {
            return x * x * x;
        }
    }
}