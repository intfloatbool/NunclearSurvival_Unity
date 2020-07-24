using System.Collections.Generic;
using NunclearGame.Enums;
using NunclearGame.Metro;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class MetroHolder : UnitySingletonBase<MetroHolder>
    {
        [Header("Station definitions")] 
        [SerializeField] private StationProperties[] _stationPropertieses;
        
        private Dictionary<string, StationProperties> _stationsDict = new Dictionary<string, StationProperties>();
        
        protected override MetroHolder GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();
            InitDict();
        }

        private void InitDict()
        {
            for (int i = 0; i < _stationPropertieses.Length; i++)
            {
                StationProperties stationProperties = _stationPropertieses[i];
                string stationName = stationProperties.Name;
                if (_stationsDict.ContainsKey(stationName))
                {
                    Debug.LogError($"Double definition station with name {stationName} !!!");
                }
                else
                {
                    _stationsDict.Add(stationName, stationProperties);
                }
            }
        }

        public StationProperties GetStationPropertiesByName(string stationNameKey)
        {
            StationProperties properties = null;
            _stationsDict.TryGetValue(stationNameKey, out properties);
            return properties;
        }

        public Color GetColorByDanger(DangerType dangerType)
        {
            if (dangerType == DangerType.VERY_LOW)
                return Color.cyan;
            if(dangerType == DangerType.LOW)
                return Color.green;
            if(dangerType == DangerType.MIDDLE)
                return Color.yellow;
            if(dangerType == DangerType.HARD)
                return Color.red;
            if(dangerType == DangerType.VERY_HARD)
                return Color.red;

            return Color.white;
        }
    }
}
