using UnityEngine;

namespace Foxworks.Components.ParticleSystemUtils
{
    [ExecuteAlways]
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesFollowCanvasVisibility : MonoBehaviour
    {
        [SerializeField] private Canvas _daddyCanvas;
        [SerializeField] private CanvasGroup _daddyCanvasGroup;
        [SerializeField] private Gradient _particleGradient;

        private ParticleSystem _ps;
        private float _visibility = -1f;

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!Mathf.Approximately(_visibility, _daddyCanvasGroup.alpha))
            {
                _visibility = _daddyCanvasGroup.alpha;
                ParticleSystem.MainModule main = _ps.main;

                Gradient gradient = new();
                GradientColorKey[] colorKeys = new GradientColorKey[_particleGradient.colorKeys.Length];
                for (int i = 0; i < colorKeys.Length; i++)
                {
                    colorKeys[i] = new GradientColorKey(_particleGradient.colorKeys[i].color, _particleGradient.colorKeys[i].time);
                }

                GradientAlphaKey[] alphakeys = new GradientAlphaKey[_particleGradient.alphaKeys.Length];
                for (int i = 0; i < alphakeys.Length; i++)
                {
                    alphakeys[i] = new GradientAlphaKey(_visibility, _particleGradient.alphaKeys[i].time);
                }

                gradient.SetKeys(colorKeys, alphakeys);
                ParticleSystem.MinMaxGradient startColor = new(gradient);
                main.startColor = startColor;
            }


            switch (_daddyCanvas.enabled)
            {
                case false when _ps.isPlaying:
                    _ps.Stop();
                    break;
                case true when !_ps.isPlaying:
                    _ps.Play();
                    break;
            }
        }
    }
}