using UnityEngine;

namespace Fox.App
{
    public class PerformanceHandler : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
    }
}