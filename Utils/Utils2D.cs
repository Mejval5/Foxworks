using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Foxworks.Utils
{
    public static class Utils2D
    {
        public static IEnumerable<RaycastHit2D> GetObjectsUnderCursor(Camera camera, bool sortByZ = true)
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue();
            Vector2 worldPosition = camera.ScreenToWorldPoint(screenPosition);
                
            // ReSharper disable once Unity.PreferNonAllocApi
            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);
            if (sortByZ == false)
            {
                return hits;
            }
            
            IEnumerable<RaycastHit2D> sortedHits = hits.OrderByDescending(hit => hit.collider.transform.position.z);
            return sortedHits;
        }
    }
}