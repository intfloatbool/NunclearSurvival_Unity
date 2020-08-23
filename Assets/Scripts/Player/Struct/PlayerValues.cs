using UnityEngine;

namespace NunclearGame.Player
{
    [System.Serializable]
    public struct PlayerValues
    {
        [SerializeField] private int _playerLvl;
        public int PlayerLvl => _playerLvl;

        [SerializeField] private int _maxHp;
        public int MaxHp => _maxHp;

        [SerializeField] private int _currentHp;
        public int CurrentHp => _currentHp;

        [SerializeField] private int _maxStamina;
        public int MaxStamina => _maxStamina;

        [SerializeField] private int _currentStamina;
        
        public int CurrentStamina => _currentStamina;

        [SerializeField] private int _rating;
        public int Rating => _rating;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerLvl">playerLvl</param>
        /// <param name="maxHp">maxHp</param>
        /// <param name="currentHp">currentHp</param>
        /// <param name="maxStamina">maxStamina</param>
        /// <param name="rating">rating</param>
        public PlayerValues(int playerLvl, int maxHp, int currentHp, int maxStamina, int currentStamina, int rating)
        {
            _playerLvl = playerLvl;
            _maxHp = maxHp;
            _currentHp = currentHp;
            _maxStamina = maxStamina;
            _currentStamina = currentStamina;
            _rating = rating;
        }
    }
}