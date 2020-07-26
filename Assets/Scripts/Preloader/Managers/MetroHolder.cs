using System;
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

        public class StationDangerIconInfo
        {
            [SerializeField] private Sprite _sprite;
            public Sprite Sprite => _sprite;
            [SerializeField] private Color _color;
            public Color Color => _color;

            public StationDangerIconInfo(Sprite sprite, Color color)
            {
                this._sprite = sprite;
                this._color = color;
            }
        }

        [SerializeField] private Sprite _dangerSprite;
        private Dictionary<DangerType, StationDangerIconInfo> _dangerIconsDict = new Dictionary<DangerType, StationDangerIconInfo>();

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

        private void InitDangerIconsDict()
        {
            var dangerTypeNames = Enum.GetNames(typeof(DangerType));
            for (int i = 0; i < dangerTypeNames.Length; i++)
            {
                var dangerType = (DangerType) i;
                _dangerIconsDict.Add(dangerType, CreateIconInfoByDangerType(dangerType));
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

        public StationDangerIconInfo GetDangerIconByType(DangerType dangerType)
        {
            StationDangerIconInfo iconInfo = null;
            _dangerIconsDict.TryGetValue(dangerType, out iconInfo);
            if (iconInfo == null)
            {
                Debug.LogError($"There is no dangerIcon for {dangerType} !");
            }

            return iconInfo;
        }
        
        private StationDangerIconInfo CreateIconInfoByDangerType(DangerType dangerType)
        {
            var colorByDanger = GetColorByDanger(dangerType);
            return new StationDangerIconInfo(_dangerSprite, colorByDanger);
        }
    }
}
