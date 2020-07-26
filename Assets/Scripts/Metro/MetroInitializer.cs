using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroInitializer : MonoBehaviour
    {
        [SerializeField] private MetroMapView[] _stations;
        public MetroMapView[] Stations => _stations;
        private Dictionary<string, MetroMapView> _mapViewsDict = new Dictionary<string, MetroMapView>();
        
        [Space(5f)]
        [Header("Runtime refs")]
        [SerializeField] private MetroMapView _currentPlayerStation;
        public MetroMapView CurrentPlayerStation => _currentPlayerStation;
        
        public event Action<MetroMapView[]> OnStationsInitialized;
        public event Action<MetroMapView[]> OnStationsUpdated;

        
        
        private void Awake()
        {
            Assert.IsNotNull(GameHelper.MetroHolder, "GameHelper.MetroHolder != null");
            if (GameHelper.MetroHolder != null)
            {
                InitStations();
                GameHelper.MetroHolder.OnPlayerLastStationUpdated += PlacePlayerAtStation;
                GameHelper.MetroHolder.OnStationDataUpdated += UpdateStationView;
            }
        }

        private void Start()
        {
            if (GameHelper.MetroHolder != null)
            {
                PlacePlayerAtStation(GameHelper.MetroHolder.PlayerLastStationKey);
            }
        }

        private void OnDestroy()
        {
            if (GameHelper.MetroHolder != null)
            {
                GameHelper.MetroHolder.OnPlayerLastStationUpdated -= PlacePlayerAtStation;
                GameHelper.MetroHolder.OnStationDataUpdated -= UpdateStationView;
            }
        }

        private void UpdateStationView(string stationKey, StationProperties stationProperties)
        {
            var mapView = GetMapViewByStationKey(stationKey);
            if (mapView != null)
            {
                mapView.InitStation(stationProperties);   
            }
        }

        private void PlacePlayerAtStation(string stationKey)
        {
            foreach (var metroMapView in _stations)
            {
                if (metroMapView.MetroNameKey.Equals(stationKey))
                {
                    metroMapView.SetPlayerIsHere(true);
                    _currentPlayerStation = metroMapView;
                }
                else
                {
                    metroMapView.SetPlayerIsHere(false);
                }
            }
        }

        private void InitStations()
        {
            for (int i = 0; i < _stations.Length; i++)
            {
                var station = _stations[i];
                if (station != null)
                {
                    var stationMetroNameKey = station.MetroNameKey;
                    if (_mapViewsDict.ContainsKey(stationMetroNameKey))
                    {
                        Debug.LogError($"Station with key {stationMetroNameKey} already defined!!");
                    }
                    else
                    {
                        var properties = GameHelper.MetroHolder.GetStationPropertiesByName(stationMetroNameKey);

                        if (properties == null)
                        {
                            Debug.LogError($"Cannot find station properties with name {stationMetroNameKey}!");
                        }
                        else
                        {
                            station.InitStation(properties);
                            _mapViewsDict.Add(stationMetroNameKey, station);
                        }
                    }
                    
                }
            }

            CheckThatAllStationsOnMapIsInitialized();
            
            OnStationsInitialized?.Invoke(_stations.ToArray());
        }

        private void CheckThatAllStationsOnMapIsInitialized()
        {
            var allStationsInMap = FindObjectsOfType<MetroMapView>().ToList();
            foreach (var metroMapView in allStationsInMap)
            {
                if (!_mapViewsDict.ContainsKey(metroMapView.MetroNameKey))
                {
                    Debug.LogError($"Station {metroMapView.MetroNameKey} not initialized in metroInitializer!!!");
                }   
            }
        }

        private MetroMapView GetMapViewByStationKey(string stationKey)
        {
            MetroMapView mapView = null;
            _mapViewsDict.TryGetValue(stationKey, out mapView);
            if (mapView == null)
            {
                Debug.LogError($"MapView with key {stationKey} is missing!");
            }

            return mapView;
        }
        
    }
}
