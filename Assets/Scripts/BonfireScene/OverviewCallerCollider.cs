using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class OverviewCallerCollider : MonoBehaviour
    {
        [SerializeField] private OverviewElement _relationOverview;
        private OverviewManager _overviewManager;

        private void Awake()
        {
            _overviewManager = FindObjectOfType<OverviewManager>();
            Assert.IsNotNull(_overviewManager, "_overviewManager != null");
        }

        private void OnMouseDown()
        {
            if (_relationOverview != null)
            {
                if (_overviewManager != null)
                {
                    _overviewManager.SetOverview(_relationOverview);
                }
            }
        }
    }
}
