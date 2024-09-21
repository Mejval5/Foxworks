using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fox.Utils
{
    public static class GraphicUtils
    {
        public static IEnumerator FadeGraphicCoroutine(this Graphic graphic, Color targetValue, float fadeTime, float fadeDelay)
        {
            graphic.enabled = true;
            Color startValue = graphic.color;

            if (Mathf.Approximately(fadeTime, 0f))
            {
                graphic.color = targetValue;
                graphic.enabled = targetValue.a > 0;
                yield break;
            }

            if (fadeDelay > 0f)
            {
                yield return new WaitForSeconds(fadeDelay);
            }
            
            float startTime = Time.time;
            while (startTime + fadeTime > Time.time)
            {
                float t = (Time.time - startTime) / fadeTime;
                graphic.color = Color.Lerp(startValue, targetValue, t);
                yield return null;
            }

            graphic.color = targetValue;
            graphic.enabled = targetValue.a > 0;
        }

        public static IEnumerator AnimateAlpha(this Graphic graphic, float targetAlpha, float animationTime)
        {
            return graphic.FadeGraphicCoroutine(new Color(graphic.color.r, graphic.color.g, graphic.color.b, targetAlpha), animationTime, 0);
        }
    }
}