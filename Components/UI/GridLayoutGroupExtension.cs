using System;
using UnityEngine;
using UnityEngine.UI;

namespace Foxworks.Components.UI
{
    [Serializable]
    public enum EnforceType
    {
        None,
        Width,
        WidthAndRatio,
        Height,
        HeightAndRatio,
        Both
    }
    
    [RequireComponent(typeof(GridLayoutGroup))]
    [ExecuteAlways]
    public class GridLayoutGroupExtension : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        [SerializeField] private Vector2Int _gridSize = new Vector2Int(1, 1);
        [SerializeField] private EnforceType _type;
        [SerializeField] private Vector2 _ratio = Vector2.one;
        
        private RectTransform _rectTransform;
        
        private void OnValidate()
        {
            UpdateLayout();
        }

        protected void OnRectTransformDimensionsChange()
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }

            if (_rectTransform == null)
            {
                _rectTransform = _gridLayoutGroup.GetComponent<RectTransform>();
            }

            if (_type is EnforceType.None)
            {
                return;
            }
            
            Vector2 targetCellSize = _gridLayoutGroup.cellSize;
            Vector2 spacing = _gridLayoutGroup.spacing;
            RectOffset padding = _gridLayoutGroup.padding;
            float ratio = _ratio.x / _ratio.y;
      
            if (_type is EnforceType.Width or EnforceType.WidthAndRatio or EnforceType.Both)
            {
                targetCellSize.x = (_rectTransform.rect.width - padding.left - padding.right - spacing.x * (_gridSize.x - 1)) / _gridSize.x;
            }
            
            if (_type is EnforceType.WidthAndRatio)
            {
                targetCellSize.y = targetCellSize.x / ratio;
            }
            
            if (_type is EnforceType.Height or EnforceType.HeightAndRatio or EnforceType.Both)
            {
                targetCellSize.y = (_rectTransform.rect.height - padding.top - padding.bottom - spacing.y * (_gridSize.y - 1)) / _gridSize.y;
            }
            
            if (_type is EnforceType.HeightAndRatio)
            {
                targetCellSize.x = targetCellSize.y * ratio;
            }
            
            _gridLayoutGroup.cellSize = targetCellSize;
        }
    }
}