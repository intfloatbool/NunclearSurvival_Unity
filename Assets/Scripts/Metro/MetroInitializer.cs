using System;
using System.Linq;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroInitializer : MonoBehaviour
    {
        [SerializeField] private MetroMapView[] _stations;
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
                    var stationName = station.MetroNameKey;
                    station.name = stationName;
                    var properties = GameHelper.MetroHolder.GetStationPropertiesByName(stationName);

                    if (properties == null)
                    {
                        Debug.LogError($"Cannot find station properties with name {stationName}!");
                    }
                    else
                    {
                        station.InitStation(properties);
                    }
                }
            }
            
            OnStationsInitialized?.Invoke(_stations.ToArray());
        }
    }
}
