using System;
using System.Collections.Generic;
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
        public event Action<MetroMapView[]> OnStationsInitialized;
        
        private void Awake()
        {
            Assert.IsNotNull(GameHelper.MetroHolder, "GameHelper.MetroHolder != null");
            if (GameHelper.MetroHolder != null)
            {
                InitStations();
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
    }
}
