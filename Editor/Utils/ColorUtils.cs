using UnityEngine;

namespace Foxworks.Editor.Utils
{
	public static class ColorUtils
	{
		public static Color ConvertStringToColor(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Color.white;
			}

			// Use the hash code as a seed for the random number generator, added constant to have prettier colors
			System.Random random = new (text.GetHashCode() + 5);

			// Generate random color components
			float hue = (float) random.NextDouble();							// all spectrum is welcome:)
			float saturation = (float) (random.NextDouble() / 2.5f + 0.2);		// away from white, but not overly saturated
			float value = (float) (random.NextDouble() / 4 + 0.7);				// away from black, but not totally white

			return Color.HSVToRGB(hue, saturation, value);
		}

		public static Color TextColorFromBackgroundColor(Color backgroundColor)
		{
			float luminance = (0.299f * backgroundColor.r + 0.587f * backgroundColor.g + 0.114f * backgroundColor.b);
			return luminance > 0.3f ? Color.black : Color.white;
		}
	}
}