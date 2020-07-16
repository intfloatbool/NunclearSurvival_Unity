using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class OverviewManager : MonoBehaviour
    {
        [SerializeField] private OverviewElement _defaultOverview;
        public event Action<OverviewElement> OnOverviewChanged;

        private OverviewElement _currentOverviewElement;

        private void Awake()
        {
            Assert.IsNotNull(_defaultOverview, "_defaultOverview != null");
            if (_defaultOverview != null)
            {
                SetOverview(_defaultOverview);
            }
        }

        public void SetOverview(OverviewElement overviewElement)
        {
            if (overviewElement == null)
            {
                Debug.LogError("Overview element is missing!");
                return;
            } 
            if (overviewElement != _currentOverviewElement)
            {
                _currentOverviewElement = overviewElement;
                OnOverviewChanged?.Invoke(_currentOverviewElement);
            }
        }
    }
}
