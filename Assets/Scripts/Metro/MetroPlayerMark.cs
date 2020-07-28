using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroPlayerMark : MonoBehaviour
    {
        private MetroController _metroController;
        private MetroInitializer _metroInitializer;

        public event Action<MetroMapView> OnPlayerMarkChanged;

        private void Awake()
        {
            _metroController = FindObjectOfType<MetroController>();
            _metroInitializer = FindObjectOfType<MetroInitializer>();
            
            Assert.IsNotNull(_metroController, "_metroController != null");
            Assert.IsNotNull(_metroInitializer, "_metroInitializer != null");

            if (_metroController != null)
            {
                _metroController.OnPlayerTransitionToStation += PlaceOnMetroView;
            }

            if (_metroInitializer != null)
            {
                _metroInitializer.OnPlayerStartPointInitialized += PlaceOnMetroView;
            }
        }

        private void OnDestroy()
        {
            if (_metroController != null)
            {
                _metroController.OnPlayerTransitionToStation -= PlaceOnMetroView;
            }

            if (_metroInitializer != null)
            {
                _metroInitializer.OnPlayerStartPointInitialized -= PlaceOnMetroView;
            }
        }

        private void PlaceOnMetroView(MetroMapView metroMapView)
        {
            transform.position = metroMapView.transform.position;
            
            OnPlayerMarkChanged?.Invoke(metroMapView);
        }
    }
}
