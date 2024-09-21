using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor.Fox.Utils
{
    public class CleaningUtils : MonoBehaviour
    {
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