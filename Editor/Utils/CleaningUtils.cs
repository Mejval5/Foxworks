using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Foxworks.Editor.Utils
{
    public class CleaningUtils : MonoBehaviour
    {
        /// <summary>
        /// Old navmesh data is not removed when the scene is reloaded.
        /// This method forces the cleanup of the navmesh data.
        /// </summary>
        [MenuItem("Tools/Editor/Force Cleanup NavMesh")]
        public static void ForceCleanupNavMesh()
        {
            if (Application.isPlaying)
            {
                return;
            }

            NavMesh.RemoveAllNavMeshData();
        }
    }
}