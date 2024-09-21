using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Foxworks.Juicing
{
	/// <summary>
	///     Components which animates the scale of the attached Game Object to make it bounce by a given curve.
	///     Mostly used for juicing UI elements, but can be also utilized in 3D for juicing.
	/// </summary>
	public class RandomBounce : MonoBehaviour
    {
        [SerializeField] private float _bounceDuration = 1;
        [SerializeField] private float _bounceMaxOffset = 0.2f;

        [SerializeField] private float _initDelay = 1;
        [SerializeField] private Vector2 _bounceDelayRange = new(1, 25);
        [SerializeField] private AnimationCurve _bounceCurve = new(new Keyframe(0f, 0f, 2f, 2f), new Keyframe(0.5f, 1.0f, -3.68f, -3.68f), new Keyframe(0.66f, -0.5f, 1.24f, 1.24f),
            new Keyframe(0.73f, 0.33f, 3.8f, 3.8f), new Keyframe(0.85f, -0.18f, -0.75f, -0.75f), new Keyframe(0.96f, 0.12f, -0.1f, -0.1f), new Keyframe(1f, 0f, -2.94f, -2.94f));

        private Vector3 _defaultScale;

        protected void Awake()
        {
            _defaultScale = transform.localScale;
        }

        protected void OnEnable()
        {
            StartCoroutine(FirstBounce());
        }

        protected void OnDisable()
        {
            StopAllCoroutines();
            transform.localScale = _defaultScale;
        }

        private IEnumerator FirstBounce()
        {
            yield return new WaitForSecondsRealtime(_initDelay);

            StartCoroutine(DoNextBounce());
        }

        private IEnumerator DoNextBounce()
        {
            float delay = Random.Range(_bounceDelayRange.x, _bounceDelayRange.y);
            yield return new WaitForSecondsRealtime(delay);

            yield return StartCoroutine(Bounce());

            FinishBounce();
        }

        private IEnumerator Bounce()
        {
            float xTime = Random.Range(0f, _bounceDuration / 2f);
            float yTime = Random.Range(0f, _bounceDuration / 2f);
            float zTime = Random.Range(0f, _bounceDuration / 2f);
            float minOffset = 1f - _bounceMaxOffset;
            float maxOffset = 1f + _bounceMaxOffset;

            Vector3 startScale = transform.localScale;
            float xTarget = Random.Range(startScale.x * minOffset, startScale.x * maxOffset);
            float yTarget = Random.Range(startScale.y * minOffset, startScale.y * maxOffset);
            float zTarget = Random.Range(startScale.z * minOffset, startScale.z * maxOffset);
            Vector3 targetScale = new(xTarget, yTarget, zTarget);

            float time = 0f;
            while (time <= _bounceDuration * 3f / 2f)
            {
                float xVal = Mathf.Clamp((time - xTime) / _bounceDuration, 0f, 1f);
                float xScaleVal = _bounceCurve.Evaluate(xVal);
                float xScale = Mathf.Lerp(startScale.x, targetScale.x, xScaleVal);

                float yVal = Mathf.Clamp((time - yTime) / _bounceDuration, 0f, 1f);
                float yScaleVal = _bounceCurve.Evaluate(yVal);
                float yScale = Mathf.Lerp(startScale.y, targetScale.y, yScaleVal);

                float zVal = Mathf.Clamp((time - zTime) / _bounceDuration, 0f, 1f);
                float zScaleVal = _bounceCurve.Evaluate(zVal);
                float zScale = Mathf.Lerp(startScale.z, targetScale.z, zScaleVal);

                transform.localScale = new Vector3(xScale, yScale, zScale);

                yield return null;
                time += Time.deltaTime;
            }

            transform.localScale = startScale;
        }

        private void FinishBounce()
        {
            StartCoroutine(DoNextBounce());
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RandomBounce))]
    internal class RandomBounceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RandomBounce randomBounce = (RandomBounce)target;
            bool detectedAnimator = randomBounce.GetComponent<Animator>() != null;

            if (detectedAnimator)
            {
                EditorGUILayout.HelpBox(
                    "Animator component can sometimes lock variables on the GameObjects and prevent them from being modified by scripts. " +
                    "This component might not work properly if the Animator contains any animation clips which modify " +
                    "the transform.localScale.",
                    MessageType.Warning);
            }
        }
    }
#endif
}