using UnityEngine;

namespace Foxworks.Components.TransformUtils
{
    [ExecuteInEditMode]
    public class FollowTransformWithOffset : MonoBehaviour
    {
        [SerializeField] private Transform _transformToFollow;
        [SerializeField] private Vector3 _offset;

        protected void Update()
        {
            UpdatePosition();
        }

        protected void OnValidate()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector3 pos = _transformToFollow.position + _offset;
            transform.position = pos;
        }
    }
}