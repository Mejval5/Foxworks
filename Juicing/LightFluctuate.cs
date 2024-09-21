
using UnityEngine;

namespace Fox.Juicing
{
    public class LightFluctuate : MonoBehaviour
    {
        [SerializeField] private Vector2 _lightRange;
        [SerializeField] private Light _light;
        [SerializeField] private float _speed;
    
        [SerializeField] private bool _isDark;
        private float _seed;
    
        // Start is called before the first frame update
        private void Awake()
        {
            _seed = Random.Range(-100000f, 100000f);
        }

        private void OnEnable()
        {
            _light.enabled = _isDark;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isDark == false)
            {
                return;
            }
        
            float t = Mathf.Clamp(Mathf.PerlinNoise(_speed * Time.time, _seed), 0f, 1f);
        
            float lightVal = Mathf.Lerp(_lightRange.x, _lightRange.y, t);
            Color color = _light.color;
            color.a = lightVal;
            _light.color = color;
        }
    }
}
