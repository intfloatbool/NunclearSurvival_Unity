using System;
using UnityEngine;

namespace Player
{
    public class GlobalPlayer : UnitySingletonBase<GlobalPlayer>
    {
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

        public event Action<string> OnPlayerNameUpdated = (playerName) => { };
    
        [SerializeField] private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;
    
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
        }
    }
}