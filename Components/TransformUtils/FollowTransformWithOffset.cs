using UnityEngine;

namespace Fox.Components.TransformUtils
{
    [ExecuteInEditMode]
    public class FollowTransformWithOffset : MonoBehaviour
    {
        [SerializeField] private Transform _transformToFollow;
        [SerializeField] private Vector3 _offset;

        private void UpdatePosition()
        {
            Vector3 pos = _transformToFollow.position + _offset;
            transform.position = pos;
        }
        
        protected void Update()
        {
            UpdatePosition();
        }
        
        protected void OnValidate()
        {
            UpdatePosition();
        }
    }
}
