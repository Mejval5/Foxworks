
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fox.Utils
{
    public class MathUtils
    {
        public static float EaseInOutCubic(float x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }

        public static int RandomSign()
        {
            return (int) Mathf.Pow(-1, Random.Range(1, 3));
        }
        
        public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
        {
            float u;
            float s;

            do
            {
                u = 2.0f * Random.value - 1.0f;
                float v = 2.0f * Random.value - 1.0f;
                s = u * u + v * v;
            } while (s >= 1.0f);

            // Standard Normal Distribution
            float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);

            // Normal Distribution centered between the min and max value
            // and clamped following the "three-sigma rule"
            float mean = (minValue + maxValue) / 2.0f;
            float sigma = (maxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
        }
    }
}