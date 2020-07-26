using System;
using System.Collections.Generic;
using NunclearGame.Enums;
using NunclearGame.Metro;
using NunclearGame.Static;
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
        public Dictionary<string, StationProperties> StationsDict => _stationsDict;

        [Space(5f)] 
        [SerializeField] private Color _playerHereColor = Color.blue;
        public Color PlayerHereColor => _playerHereColor;
        
        [SerializeField] private Color _playerNotHereColor = Color.white;
        public Color PlayerNotHereColor => _playerNotHereColor;

        [Space(5f)] 
        [SerializeField] private string _playerDefaultStationKey;

        [SerializeField] private StationData _defaultStationData;
        
        private string _playerLastStationKey;
        public string PlayerLastStationKey => _playerLastStationKey;

        /// <summary>
        /// Arg#0 - StationKey
        /// Arg#1 - StationProperties ref from holder
        /// </summary>
        public event Action<string, StationProperties> OnStationDataUpdated;

        /// <summary>
        /// Arg#0 - Current player station
        /// </summary>
        public event Action<string> OnPlayerLastStationUpdated;
        
        protected override MetroHolder GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();
            InitStationDict();
            InitColorsDict();
            InitDangerIconsDict();
        }

        private void Start()
        {
            //Data loading
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadAllStationData();
            LoadLastPlayerStation();
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

        private void LoadAllStationData()
        {
            foreach (var stationKey in _stationsDict.Keys)
            {
                _stationsDict[stationKey].StationData = GameHelper.InfoProvider.LoadStationDataByKey(stationKey);
            }
        }

        private void LoadLastPlayerStation()
        {
            _playerLastStationKey = GameHelper.InfoProvider.LoadCurrentPlayerStationKey();
            if (string.IsNullOrEmpty(_playerLastStationKey))
            {
                _playerLastStationKey = _playerDefaultStationKey;
                SetLastPlayerStation(_playerLastStationKey);
                UpdateStationData(_playerLastStationKey, _defaultStationData);
                Debug.Log("Last metro station is not defined, load default! - " + _playerLastStationKey);
            }
        }

        public void SetLastPlayerStation(string stationKey)
        {
            StationData lastStationData;
            if (!_stationsDict.ContainsKey(stationKey))
            {
                Debug.LogError("There is no station defined for key" + stationKey);
                return;
            }

            StationProperties stationProperties = _stationsDict[stationKey];

            //To copy last values
            lastStationData = stationProperties.StationData;
            
            _playerLastStationKey = stationKey;
            GameHelper.InfoProvider.SetCurrentPlayerStationKey(stationKey);
            
            StationData updatedStationData =
                new StationData
                {
                    IsClear = true
                };
            GameHelper.InfoProvider.UpdateStationData(stationKey, updatedStationData);

            stationProperties.StationData = updatedStationData;
            OnStationDataUpdated?.Invoke(stationKey, stationProperties);
            OnPlayerLastStationUpdated?.Invoke(_playerLastStationKey);
        } 

        public void UpdateStationData(string stationNameKey, StationData stationData)
        {
            var propertiesByName = GetStationPropertiesByName(stationNameKey);
            if (propertiesByName == null)
            {
                Debug.LogError("There is Properties for station: " + stationNameKey);
                return;
            }
            propertiesByName.StationData = stationData;
            if (GameHelper.GlobalPlayer == null)
            {
                Debug.LogError("GlobalPlayer is missing!");
                return;
            }
            
            GameHelper.GlobalPlayer.PlayerInfoProvider.UpdateStationData(stationNameKey, stationData);
            OnStationDataUpdated?.Invoke(stationNameKey, propertiesByName);
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

        //Debug
        public void MakeAllStationsCleared()
        {
            foreach (var stationKey in _stationsDict.Keys)
            {
                var stationData = 
                    new StationData
                    {
                        IsClear = true
                    };
                UpdateStationData(stationKey, stationData);
            }
        }
        
        private StationDangerIconInfo CreateIconInfoByDangerType(DangerType dangerType)
        {
            var colorByDanger = GetColorByDanger(dangerType);
            return new StationDangerIconInfo(_dangerSprite, colorByDanger);
        }
    }
}
