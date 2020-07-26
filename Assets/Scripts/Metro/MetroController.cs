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

        public event Action<MetroMapView> OnPlayerTransitionToStation;

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
            }
            

        }
    }
}
