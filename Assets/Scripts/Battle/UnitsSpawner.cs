using System;
using UnityEngine;

namespace NunclearGame.Battle
{
    public class UnitsSpawner : MonoBehaviour
    {
        
        [SerializeField] private UnitSpawnInfo[] _spawnData;
        public UnitSpawnInfo[] SpawnData
        {
            get => _spawnData;
            set => _spawnData = value;
        }
        
        public event Action<GameUnit> OnUnitSpawned;
        
        [Space(5f)]
        [Header("DANGER ZONE: DEBUG")]
        [SerializeField] private bool _isSpawnAtStart;

        private void Start()
        {
            if (_isSpawnAtStart)
            {
                SpawnUnits();
            }
        }

        public void SpawnUnits()
        {
            for (int i = 0; i < _spawnData.Length; i++)
            {
                SpawnUnit(_spawnData[i]);
            }
        }

        private void SpawnUnit(UnitSpawnInfo unitSpawnInfo)
        {
            if (unitSpawnInfo == null)
            {
                Debug.LogError($"{nameof(UnitSpawnInfo)} is missing!");
                return;
            }

            if (unitSpawnInfo.GameUnitPrefab == null)
            {
                Debug.LogError("UnitPrefab is missing!");
                return;
            }

            Vector3 spawnPosition = Vector3.zero;
            
            if (unitSpawnInfo.UnitSpawnPoint != null)
            {
                spawnPosition = unitSpawnInfo.UnitSpawnPoint.position;
            }
            else
            {
                Debug.LogWarning($"SpawnPoint is missing! Place {unitSpawnInfo.GameUnitPrefab.name} at zero pos.");
            }

            GameUnit unitInstance = Instantiate(unitSpawnInfo.GameUnitPrefab, unitSpawnInfo.UnitSpawnPoint);
            unitInstance.transform.position = spawnPosition;
            
            OnUnitSpawned?.Invoke(unitInstance);
        }
    }
}
