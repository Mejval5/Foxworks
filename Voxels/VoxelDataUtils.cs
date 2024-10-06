using UnityEngine;

namespace Foxworks.Voxels
{
    public static class VoxelDataUtils
    {
        public const int ValueBits = 10; // First bits store value as 0f-1f
        public const int Values = 1 << ValueBits;
        public const int ValueMask = (1 << ValueBits) - 1;
        public const float ValueMultiplier = 1.0f / ValueMask;
        
        public const int VertexIdBits = 22; // We consider next bits for vertex id, this is used to encode special data
        public const int VertexIdMask = (1 << VertexIdBits) - 1;
        
        public static int ConvertTo22Bit(this Color color)
        {
            // Adjust the component bit sizes
            int r = Mathf.Clamp((int)(color.r * 127), 0, 127);  // 7 bits for Red
            int g = Mathf.Clamp((int)(color.g * 255), 0, 255);  // 8 bits for Green
            int b = Mathf.Clamp((int)(color.b * 127), 0, 127);  // 7 bits for Blue

            // Pack the color into a 22-bit integer
            int packedColor = (r << 15) | (g << 7) | b;

            return packedColor;
        }
        
        public static int PackValueAndVertexColor(float value, Color vertexColor)
        {
            return PackValueAndVertexColor(value, vertexColor.ConvertTo22Bit());
        }
        
        public static int PackValueAndVertexColor(float value, int vertexId = 0)
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