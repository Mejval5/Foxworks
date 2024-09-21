using UnityEngine;

namespace Foxworks.Juicing
{
    /// <summary>
    ///     Fluctuates the intensity of a light.
    /// </summary>
    public class LightFluctuate : MonoBehaviour
    {
        [SerializeField] private Vector2 _lightRange;
        [SerializeField] private Light _light;
        [SerializeField] private float _speed;

        [SerializeField] private bool _isDark;

        // Update is called once per frame
        private void Update()
        {
            if (_isDark == false)
            {
                return;
            }

            float t = Mathf.Clamp(Mathf.PerlinNoise1D(_speed * Time.time), 0f, 1f);

            float lightVal = Mathf.Lerp(_lightRange.x, _lightRange.y, t);
            Color color = _light.color;
            color.a = lightVal;
            _light.color = color;
        }

        private void OnEnable()
        {
            _light.enabled = _isDark;
        }
    }
}