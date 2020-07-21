using System;
using NunclearGame.Player;
using UnityEngine;
using Player;

namespace SingletonsPreloaders
{
    public class GlobalPlayer : UnitySingletonBase<GlobalPlayer>
    {

        public static PlayerInventory Inventory => Instance?.PlayerInventory;
        public static PlayerValuesController PlayerValuesController => Instance?.ValuesController;
        
        [Header("All player data will be load from here")]
        [SerializeField] private PlayerInfoProviderBase _playerInfoProvider;
        public PlayerInfoProviderBase PlayerInfoProvider => _playerInfoProvider;
        
        [Header("Player loaded info:")] 
        [SerializeField]
        private string _playerNickName;

        public bool IsPlayerReady
        {
            get
            {
                var isPlayerHasNickname = string.IsNullOrEmpty(_playerNickName) == false;
                return isPlayerHasNickname;
            }
        }
        
        public string PlayerNickName
        {
            get
            {
                if (!_playerInfoProvider.LoadPlayerName().Equals(_playerNickName))
                {
                    _playerNickName = _playerInfoProvider.LoadPlayerName();
                }

                return _playerNickName;
            }
            set
            {
                _playerInfoProvider.SetPlayerName(value);
                _playerNickName = _playerInfoProvider.LoadPlayerName();
                OnPlayerNameUpdated(_playerNickName);
            }
        }

        [SerializeField] private PlayerValues _playerValuesDebugInfo;
        private PlayerValues? _currentValues;
        public PlayerValues PlayerValues
        {
            get
            {
                if (_currentValues == null)
                {
                    _currentValues = _playerInfoProvider.LoadPlayerValues();
                    _playerValuesDebugInfo = _currentValues.Value;
                }
                
                return _currentValues.Value;
            }
            set
            {
                _playerInfoProvider.SavePlayerValues(value);
                _currentValues = _playerInfoProvider.LoadPlayerValues();
                _playerValuesDebugInfo = _currentValues.Value;
                OnPlayerValuesUpdated?.Invoke(_currentValues.Value);
            }
        }

        public event Action<PlayerValues> OnPlayerValuesUpdated;
        
        public event Action<string> OnPlayerNameUpdated = (playerName) => { };
    
        [SerializeField] private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;
        
        public PlayerValuesController ValuesController { get; private set; }
        
        protected override GlobalPlayer GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();
            InitializeDataFromLoader();
        }

        private void InitializeDataFromLoader()
        {
            _playerNickName = _playerInfoProvider.LoadPlayerName();
            _playerInventory = _playerInfoProvider.LoadInventory();
            
            //values
            ValuesController = new PlayerValuesController(this);
            _currentValues = _playerInfoProvider.LoadPlayerValues();
            _playerValuesDebugInfo = _currentValues.Value;
        }

        public string GetValueFromCurrentProvider(string key) {
            return _playerInfoProvider.GetValue(key);
        }

        public void SetValueFromCurrentProvider(string key, string value) {
            this._playerInfoProvider.SetValue(key, value);
        }
    }
}