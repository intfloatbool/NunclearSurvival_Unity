using System;
using Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class CameraOverviewListener : MonoBehaviour
    {
        [SerializeField] private TransformFollower _cameraTransformFollower;
        [SerializeField] private OverviewManager _overviewManager;
        private void Awake()
        {
            Assert.IsNotNull(_cameraTransformFollower, "_cameraTransformFollower != null");
            Assert.IsNotNull(_overviewManager, "_overviewManager != null");
            if (_overviewManager != null)
            {
                _overviewManager.OnOverviewChanged += OnOverviewChanged;
            }
        }

        private void OnDestroy()
        {
            if (_overviewManager != null)
            {
                _overviewManager.OnOverviewChanged -= OnOverviewChanged;
            } 
        }

        private void OnOverviewChanged(OverviewElement overviewElement)
        {
            if (_cameraTransformFollower != null)
            {
                _cameraTransformFollower.FollowTransform = overviewElement.CameraPos;
            } 
        }
    }
}
