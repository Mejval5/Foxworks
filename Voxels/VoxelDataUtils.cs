using UnityEngine;

namespace VoxelPainter.Rendering.Utils
{
    public static class VoxelDataUtils
    {
        public const int ValueBits = 10; // First bits store value as 0f-1f
        public const int Values = 1 << ValueBits;
        public const int ValueMask = (1 << ValueBits) - 1;
        public const float ValueMultiplier = 1.0f / ValueMask;
        
        public const int VertexIdBits = 8; // We consider next bits for vertex id, this is used to encode special data
        public const int VertexIdMask = (1 << VertexIdBits) - 1; // 255 (0xFF), mask to get 8 bits
        
        public static int PackValueAndVertexId(float value, int vertexId = 0)
        {
            value = Mathf.Clamp01(value);
            
            int valueInt = (int) (value * ValueMask) & ValueMask;
            int vertexIdInt = (vertexId & VertexIdMask) << ValueBits;
            return vertexIdInt | valueInt;
        }
        
        // Unpack the float value from the packed integer
        public static float UnpackValue(int packedValue)
        {
            // Extract the 10-bit value using the mask
            int valueInt = packedValue & ValueMask;

            // Convert the 10-bit integer back into a float in the 0-1 range
            return valueInt * ValueMultiplier;
        }

        // Unpack the vertex ID from the packed integer
        public static int UnpackVertexId(int packedValue)
        {
            // Extract the vertex ID by shifting right by ValueBits (to get rid of the value)
            return (packedValue >> ValueBits) & VertexIdMask;
        }
    }
}