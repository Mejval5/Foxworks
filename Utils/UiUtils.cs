using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxworks.Utils
{
    public static class UiUtils
    {
        public static bool IsPointerOverUi()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}