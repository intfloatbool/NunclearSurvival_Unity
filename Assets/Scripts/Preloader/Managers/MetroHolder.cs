using System.Collections.Generic;
using NunclearGame.Enums;
using NunclearGame.Metro;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class MetroHolder : UnitySingletonBase<MetroHolder>
    {
        
        [System.Serializable]
        private struct ColorByDanger
        {
            public DangerType DangerType;
            public Color Color;
        }

        [SerializeField] private ColorByDanger[] _colorsByDangerous;
        private Dictionary<DangerType, Color> _colorsByDangerousDict = new Dictionary<DangerType, Color>();

        [Space(5f)]
        [Header("Station definitions")] 
        [SerializeField] private StationProperties[] _stationPropertieses;
        
        private Dictionary<string, StationProperties> _stationsDict = new Dictionary<string, StationProperties>();
        
        protected override MetroHolder GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();
            InitStationDict();
            InitColorsDict();
        }

        private void InitStationDict()
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

        private void InitColorsDict()
        {
            for (int i = 0; i < _colorsByDangerous.Length; i++)
            {
                ColorByDanger colorByDanger = _colorsByDangerous[i];
                if (_colorsByDangerousDict.ContainsKey(colorByDanger.DangerType))
                {
                    Debug.LogError($"Color for dangerType: {colorByDanger.DangerType} already defined!");   
                }
                else
                {
                    _colorsByDangerousDict.Add(colorByDanger.DangerType, colorByDanger.Color);
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
            if (_colorsByDangerousDict.ContainsKey(dangerType))
                return _colorsByDangerousDict[dangerType];

            return Color.black;
        }
    }
}
