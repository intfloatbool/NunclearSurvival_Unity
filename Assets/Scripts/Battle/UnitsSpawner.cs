using System;
using System.Collections;
using UnityEngine;

namespace NunclearGame.Battle
{
    public class UnitsSpawner : MonoBehaviour
    {
        [SerializeField] private bool _isForceRotateByPoint;
        [SerializeField] private UnitSpawnInfo[] _spawnData;
        public UnitSpawnInfo[] SpawnData
        {
            get => _spawnData;
            set => _spawnData = value;
        }
        
        public event Action<GameUnit> OnUnitSpawned;
        public event Action OnSpawnDone;
        
        [Space(5f)]
        [Header("DANGER ZONE: DEBUG")]
        [SerializeField] private bool _isSpawnAtStart;
        [SerializeField] private float _spawnDelay = 1.5f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_spawnDelay);
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
            
            OnSpawnDone?.Invoke();
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
            Quaternion rotation = unitSpawnInfo.GameUnitPrefab.transform.rotation;
            if (unitSpawnInfo.UnitSpawnPoint != null)
            {
                spawnPosition = unitSpawnInfo.UnitSpawnPoint.position;
                if (_isForceRotateByPoint)
                {
                    rotation = unitSpawnInfo.UnitSpawnPoint.rotation;
                }
            }
            else
            {
                Debug.LogWarning($"SpawnPoint is missing! Place {unitSpawnInfo.GameUnitPrefab.name} at zero pos.");
            }

            GameUnit unitInstance = Instantiate(unitSpawnInfo.GameUnitPrefab, unitSpawnInfo.UnitSpawnPoint);
            unitInstance.transform.position = spawnPosition;
            unitInstance.transform.rotation = rotation;
            
            OnUnitSpawned?.Invoke(unitInstance);
        }
    }
}
