using UnityEngine;

namespace Foxworks.Components.TransformUtils
{
    [ExecuteInEditMode]
    public class FollowScaleSettings : MonoBehaviour
    {
        public Transform FollowThis;

        protected void OnEnable()
        {
            UpdateSettings();
        }

        protected void OnValidate()
        {
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            if (FollowThis != null)
            {
                transform.localScale = FollowThis.localScale;
            }
        }
    }
}