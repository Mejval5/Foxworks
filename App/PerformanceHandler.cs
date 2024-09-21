using UnityEngine;

namespace Foxworks.App
{
    public class PerformanceHandler : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
    }
}