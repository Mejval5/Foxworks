using System;
using Cinemachine;
using UnityEngine;

namespace Foxworks.Components.CameraUtils
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class OrbitalCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _centerPoint;
        
        [SerializeField] private float _startDistance = 70;
        [SerializeField] private float _downAngle = 45f;
        
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private Vector2 _dragSpeed = Vector2.one;
        
        
        private float _currentAngle = 0f;
        private float _distance = 10f;

        private bool _dragging = false;
        
        private CinemachineVirtualCamera _virtualCamera;

        private void OnEnable()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _virtualCamera.LookAt = _centerPoint;
            
            _distance = _startDistance;
        }

        private void Update()
        {
            if (_centerPoint == null)
            {
                return;
            }
            
            HandleZooming();
            
            HandleDragging();
            
            CalculatePosition();
        }
        
        private void CalculatePosition()
        {
            Vector3 position = _centerPoint.position;
            position += Quaternion.Euler(_downAngle, _currentAngle, 0) * Vector3.back * _distance;
            transform.position = position;
        }

        private void HandleZooming()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _distance -= Input.mouseScrollDelta.y * _zoomSpeed;
            }
        }

        private void HandleDragging()
        {
            if (Input.GetMouseButtonDown(2))
            {
                _dragging = true;
            }
            
            if (Input.GetMouseButtonUp(2))
            {
                _dragging = false;
            }

            if (_dragging == false)
            {
                return;
            }

            _currentAngle += Input.GetAxis("Mouse X") * _dragSpeed.x;
            _downAngle -= Input.GetAxis("Mouse Y") * _dragSpeed.y;
            _downAngle = Mathf.Clamp(_downAngle, 1, 89);
        }

        private void RecalculatePosition()
        {
            
        }
    }
}