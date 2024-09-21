using UnityEngine;

namespace Foxworks.Utils
{
    public static class RenderTextureUtils
    {
        /// <summary>
        ///     Converts a RenderTexture to a Texture2D.
        /// </summary>
        /// <param name="renderTexture"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Texture2D ToTexture2D(this RenderTexture renderTexture, TextureFormat format = TextureFormat.RGBA32)
        {
            Texture2D tex = new(renderTexture.width, renderTexture.height, format, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            return tex;
        }

        /// <summary>
        ///     Converts a RenderTexture to a Texture2D.
        /// </summary>
        /// <param name="renderTexture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Texture2D ToTexture2D(this RenderTexture renderTexture, int width, int height, TextureFormat format = TextureFormat.RGBA32)
        {
            Texture2D tex = new(width, height, format, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}