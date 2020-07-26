using System;
using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroController : MonoBehaviour
    {
        private MetroInitializer _metroInitializer;
        private MetroHolder _metroHolder;
        private MetroMapView _lastMetroMapView;

        public bool IsInteractByClicks { get; set; } = true;
        
        public event Action<MetroMapView> OnPlayerTransitionToStation;
        public event Action<MetroMapView> OnPlayerTryEnterStation;

        private void Awake()
        {
            _metroInitializer = FindObjectOfType<MetroInitializer>();
            Assert.IsNotNull(_metroInitializer, "_metroInitializer != null");
            
            _metroHolder = GameHelper.MetroHolder;
            Assert.IsNotNull(_metroHolder, "_metroHolder != null");
        }

        public void OnPlayerClickOnMetro(MetroMapView metroMapView)
        {
            TryTransitionOnMetroStation(metroMapView);
        }

        private void TryTransitionOnMetroStation(MetroMapView metroMapView)
        {
            if (!IsInteractByClicks)
                return;
            
            //TODO: Complete transition logic, checking for clearing and so on..
            var currentPlayerStation = _metroInitializer.CurrentPlayerStation;
            if (metroMapView == currentPlayerStation)
            {
                return;
            }
            bool isNearStation = currentPlayerStation.IsRelationStation(metroMapView);
            bool isClearStation = metroMapView.StationProperties.StationData.IsClear;

            if (!isNearStation)
            {
                Debug.Log("Station is too far.");
                return;
            }
            
            if (isClearStation)
            {
                _metroHolder.SetLastPlayerStation(metroMapView.MetroNameKey);
                _lastMetroMapView = metroMapView;
                OnPlayerTransitionToStation?.Invoke(metroMapView);
            }
            else
            {
                
                //TODO: Entering dialog
                Debug.Log("Station is not clear. Open transition dialog.");
                OnPlayerTryEnterStation?.Invoke(metroMapView);
            }
            

        }
    }
}
