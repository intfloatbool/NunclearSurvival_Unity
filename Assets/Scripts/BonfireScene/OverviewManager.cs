using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class OverviewManager : MonoBehaviour
    {
        [SerializeField] private OverviewElement[] _allElements;
        [SerializeField] private OverviewElement _defaultOverview;
        public event Action<OverviewElement> OnOverviewChanged;

        private OverviewElement _currentOverviewElement;

        private void Awake()
        {
            Assert.IsNotNull(_defaultOverview, "_defaultOverview != null");
            ShowDefaultOverview();
        }

        public void ShowDefaultOverview()
        {
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
                SetupElements(_currentOverviewElement);
                OnOverviewChanged?.Invoke(_currentOverviewElement);
            }
        }

        private void SetupElements(OverviewElement overviewElement)
        {
            for (int i = 0; i < _allElements.Length; i++)
            {
                var element = _allElements[i];
                if (element != null)
                {
                    element.SetActiveRelativeObjects(overviewElement == element);
                }
            }
        }
    }
}
