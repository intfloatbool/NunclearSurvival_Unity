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
        public string PlayerNickName => _playerNickName;
    
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