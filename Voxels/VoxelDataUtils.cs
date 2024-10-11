using UnityEngine;

namespace Foxworks.Voxels
{
    public static class VoxelDataUtils
    {
        public const int ValueBits = 11; // First bits store value as 0f-1f
        public const int Values = 1 << ValueBits;
        public const int ValueMask = (1 << ValueBits) - 1;
        public const float ValueMultiplier = 1.0f / ValueMask;
        
        public const int RedBits = 7; // First bits store red color component
        public const int GreenBits = 7; // Next bits store green color component
        public const int BlueBits = 7; // Next bits store blue color component
        public const int RedMask = (1 << RedBits) - 1;
        public const int GreenMask = (1 << GreenBits) - 1;
        public const int BlueMask = (1 << BlueBits) - 1;
        
        
        
        public const int ColorBits = RedBits + GreenBits + BlueBits; // We consider next bits for vertex id, this is used to encode special data
        public const int ColorsBitsMask = (1 << ColorBits) - 1;
        
        public static int ConvertToPackedColor(this Color color)
        {
            // Adjust the component bit sizes
            int r = Mathf.Clamp(Mathf.RoundToInt(color.r * RedMask), 0, RedMask);
            int g = Mathf.Clamp(Mathf.RoundToInt(color.g * GreenMask), 0, GreenMask);
            int b = Mathf.Clamp(Mathf.RoundToInt(color.b * BlueMask), 0, BlueMask);

            // Pack the color into a single integer
            int packedColor = (r << (BlueBits + GreenBits)) | (g << BlueBits) | b;

            return packedColor;
        }
        
        public static int PackValueAndVertexColor(float value, Color vertexColor)
        {
            return PackValueAndVertexColor(value, vertexColor.ConvertToPackedColor());
        }
        
        public static int PackValueAndVertexColor(float value, int colorInt = 0)
        {
            value = Mathf.Clamp01(value);
            
            int valueInt = (int) (value * ValueMask) & ValueMask;
            int vertexIdInt = (colorInt & ColorsBitsMask) << ValueBits;
            return vertexIdInt | valueInt;
        }
        
        public static int PackNativeValueAndVertexColor(int valueInt, Color vertexColor)
        {
            valueInt &= ValueMask;
            int vertexId = vertexColor.ConvertToPackedColor();
            int vertexIdInt = (vertexId & ColorsBitsMask) << ValueBits;
            return vertexIdInt | valueInt;
        }
        
        public static int PackValueAndNativeVertexColor(float value, int colorInt = 0)
        {
            int valueInt = (int) (value * ValueMask) & ValueMask;
            int vertexIdInt = (colorInt & ColorsBitsMask) << ValueBits;
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
        public static int UnpackColorInt(int packedValue)
        {
            // Extract the vertex ID by shifting right by ValueBits (to get rid of the value)
            return (packedValue >> ValueBits) & ColorsBitsMask;
        }
        
        public static Color UnpackColor(int packedValue)
        {
            packedValue = UnpackColorInt(packedValue);
            return new Color(
                ((packedValue >> (BlueBits + GreenBits)) & RedMask) / (float) RedMask,
                ((packedValue >> BlueBits) & GreenMask) / (float) GreenMask,
                (packedValue & BlueMask) / (float) BlueMask
            );
        }
    }
}