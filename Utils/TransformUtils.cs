using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Foxworks.Utils
{
    public static class TransformUtils
    {
        /// <summary>
        ///     Destroys all children of the transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        public static IEnumerator AnimateScale(this Transform transform, Vector3 targetScale, float animationTime, Func<float, float> easingFunction)
        {
            Vector3 startScale = transform.localScale;
            
            if (Mathf.Approximately(animationTime, 0f))
            {
                transform.localScale = targetScale;
                yield break;
            }

            float startTime = Time.time;
            while (startTime + animationTime > Time.time)
            {
                float t = (Time.time - startTime) / animationTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, easingFunction(t));
                yield return null;
            }

            transform.localScale = targetScale;
        }

        /// <summary>
        ///   Animate the scale of a transform linearly.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="targetScale"></param>
        /// <param name="animationTime"></param>
        /// <param name="easingType"></param>
        /// <returns></returns>
        public static IEnumerator AnimateScale(this Transform transform, Vector3 targetScale, float animationTime, EasingType easingType = EasingType.Linear)
        {
            return transform.AnimateScale(targetScale, animationTime, EasingUtils.GetEasingFunction(easingType));
        }
    }
}