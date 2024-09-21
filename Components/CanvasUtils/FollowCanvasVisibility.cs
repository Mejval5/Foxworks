using UnityEngine;

namespace Foxworks.Components.CanvasUtils
{
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
    public class FollowCanvasVisibility : MonoBehaviour
    {
        [SerializeField] private Canvas _daddyCanvas;
        [SerializeField] private CanvasGroup _daddyCanvasGroup;
        private Canvas _canvas;

        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            if (_daddyCanvasGroup != null)
            {
                _canvasGroup.alpha = _daddyCanvasGroup.alpha;
            }

            if (_canvas == null)
            {
                return;
            }

            _canvas.enabled = _daddyCanvas.enabled switch
            {
                false when _canvas.enabled => false,
                true when !_canvas.enabled => true,
                _ => _canvas.enabled
            };
        }
    }
}