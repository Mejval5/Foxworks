using UnityEngine;
using Random = System.Random;

namespace Foxworks.Utils
{
    public static class RandomUtils
    {
        public static float Value(this Random random, float min, float max)
        {
            if (min > max)
            {
                (min, max) = (max, min);
            }
            return (float) random.NextDouble() * (max - min) + min;
        }
        
        public static float Value(this Random random)
        {
            return (float) random.NextDouble();
        }
        
        public static Vector2 RandomVector2(this Random random)
        {
            float x = random.Value() - 0.5f;
            float y = random.Value() - 0.5f;
            return new Vector2(x, y).normalized;
        }
        
        public static Vector2 RandomVector3(this Random random)
        {
            float x = random.Value() - 0.5f;
            float y = random.Value() - 0.5f;
            float z = random.Value() - 0.5f;
            return new Vector3(x, y, z).normalized;
        }
    }
}