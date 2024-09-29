using UnityEngine;

namespace Foxworks.Utils
{
    public static class ScreenUtils
    {
        public static bool IsMouseInsideScreen
        {
            get
            {
                Vector2 mouseScreenPosition = Input.mousePosition;

                return mouseScreenPosition is { x: >= 0, y: >= 0 }
                       && mouseScreenPosition.x < Screen.width
                       && mouseScreenPosition.y < Screen.height;
            }
        }
    }
}