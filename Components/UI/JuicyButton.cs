using Foxworks.Sound;
using Foxworks.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Foxworks.Components.UI
{
    /// <summary>
    ///     This is my custom button script. It creates nice and juicy animation when the player clicks.
    ///     <para>If the user moves cursor outside of the button before clicking up then the button cancels.</para>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        [SerializeField] private bool _interactable = true;
        
        [Header("Parameters")]
        [SerializeField] private float _disableAlpha = 0.5f;
        
        [Header("Advanced Settings")]
        [SerializeField] private EasingType _easingType = EasingType.ElasticOut;
        [SerializeField] private float _minSize = 0.9f;
        [SerializeField] private float _maxSize = 1.1f;
        [SerializeField] private float _animationTime = 0.1f;
        [SerializeField] private float _cancelDistancePercentage = 50f;
        
        [Header("Sound")]
        [SerializeField] private AudioClip _clickedSound;
        
        [HideInInspector] public UnityEvent Clicked = new ();
        
        private Vector3 _originalScale;
        private RectTransform _rect;
        
        private Vector3 _startPos;
        private bool _currentlyPressed;
        private Vector2 _cancelDistance;
        private bool _currentlyHovered = false;

        private Canvas _mainCanvas;
        private CanvasGroup _canvasGroup;
        
        private Coroutine _currentAnimation;
        
        public bool Interactable
        {
            get => _interactable;
            set => _interactable = value;
        }
        
        public float DisableAlpha
        {
            get => _disableAlpha;
            set => _disableAlpha = value;
        }
        
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _originalScale = _rect.localScale;
            _originalScale.z = 1;
            
            _canvasGroup = GetComponent<CanvasGroup>();
            _mainCanvas = GetComponentInParent<Canvas>().rootCanvas;
        }

        private void OnEnable()
        {
            _currentlyHovered = false;
            _currentlyPressed = false;
            CancelCurrentAnimation();
            ResetVisual();
        }

        private void ResetVisual()
        {
            _rect.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
        }

        private void CancelCurrentAnimation()
        {
            if (_currentAnimation == null)
            {
                return;
            }

            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }
        
        /// <summary>
        /// Necessary for the button to work with ScrollRect and similar things
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            IBeginDragHandler parentDragHandler = transform.parent.GetComponentInParent<IBeginDragHandler>();
            parentDragHandler?.OnBeginDrag(eventData);
        }
        
        /// <summary>
        /// Necessary for the button to work with ScrollRect and similar things
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            IEndDragHandler parentDragHandler = transform.parent.GetComponentInParent<IEndDragHandler>();
            parentDragHandler?.OnEndDrag(eventData);
        }

        /// <summary>
        ///     This is a check if the user moved cursor outside of the button while holding down button.
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            IDragHandler parentDragHandler = transform.parent.GetComponentInParent<IDragHandler>();
            parentDragHandler?.OnDrag(eventData);

            if (_currentlyPressed == false || _interactable == false)
            {
                return;
            }

            float diffX = eventData.position.x - _startPos.x;
            float diffY = eventData.position.y - _startPos.y;

            Vector2 distanceVector = new(Mathf.Abs(diffX), Mathf.Abs(diffY));

            bool cancelX = distanceVector.x > _cancelDistance.x;
            bool cancelY = distanceVector.y > _cancelDistance.y;

            if (!cancelX && !cancelY)
            {
                return;
            }

            _currentlyPressed = false;
            AnimateToSize(1f);
        }

        /// <summary>
        ///     Gets called when user clicks down on the button
        /// </summary>
        public void OnPointerDown(PointerEventData _)
        {
            if (_interactable == false)
            {
                return;
            }
            
            _startPos = RectTransformUtility.WorldToScreenPoint(_mainCanvas.worldCamera, _rect.position);
            _currentlyPressed = true;
            
            Rect rect = _rect.rect;
            _cancelDistance = _cancelDistancePercentage / 100f * new Vector2(rect.width, rect.height);
            
            AnimateToSize(_minSize);
        }

        /// <summary>
        ///     Gets called when user clicks up after clicking down
        /// </summary>
        public void OnPointerUp(PointerEventData _)
        {
            if (!_currentlyPressed)
            {
                return;
            }
            
            _currentlyPressed = false;
            float targetSize = _currentlyHovered ? _maxSize : 1;
            AnimateToSize(targetSize);

            if (_interactable == false)
            {
                return;
            }

            Clicked.Invoke();
            PlaySound();
        }

        private void AnimateToSize(float targetSize)
        {
            CancelCurrentAnimation();
            _currentAnimation = StartCoroutine(transform.AnimateScale(_originalScale * targetSize, _animationTime, _easingType));
        }

        private void PlaySound()
        {
            if (_clickedSound != null && SoundManager.shared != null)
            {
                SoundManager.shared.PlaySfx(_clickedSound);
            }
        }
        
        private void Update()
        {
            ToggleInteractableState();
        }


        private void ToggleInteractableState()
        {
            _canvasGroup.alpha = _interactable ? 1f : _disableAlpha;
        }

        protected void OnDisable()
        {
            CancelCurrentAnimation();
            _rect.localScale = new Vector3(_originalScale.x, _originalScale.y, 1);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentlyHovered = true;
            
            if (_interactable == false)
            {
                return;
            }
            
            if (_currentlyPressed)
            {
                return;
            }
            
            AnimateToSize(_maxSize);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentlyHovered = false;
            
            if (_interactable == false)
            {
                return;
            }
            
            if (_currentlyPressed)
            {
                return;
            }
            
            AnimateToSize(1f);
        }
    }
}