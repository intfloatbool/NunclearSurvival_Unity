using UnityEngine;

namespace NunclearGame.Battle
{
    [System.Serializable]
    public class UnitSpawnInfo
    {
        [SerializeField] private GameUnit _gameUnitPrefab;
        public GameUnit GameUnitPrefab
        {
            get => _gameUnitPrefab;
            set => _gameUnitPrefab = value;
        }

        [SerializeField] private Transform _unitSpawnPoint;
        public Transform UnitSpawnPoint
        {
            get => _unitSpawnPoint;
            set => _unitSpawnPoint = value;
        }
    }
}
