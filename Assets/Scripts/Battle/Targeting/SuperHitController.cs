using System;
using NunclearGame.Battle.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class SuperHitController : MonoBehaviour
    {
        [SerializeField] private UnitsSpawner _spawner;
        [SerializeField] private SuperHitAim _superHitAim;

        private GameUnit _targetUnit;
        public SuperHitAim SuperHitAim => _superHitAim;
        private Targetable _currentTargetable;
        
        public bool IsAimInProcess
        {
            get
            {
                if (_superHitAim == null)
                    return false;
                return _superHitAim.IsOnProcess;
            }
        }
        
        private void Awake()
        {
            if (_spawner == null)
            {
                _spawner = FindObjectOfType<UnitsSpawner>();   
            }
            Assert.IsNotNull(_spawner, "_spawner != null");
            if (_spawner != null)
            {
                _spawner.OnUnitSpawned += GetTarget;
            }

            if (_superHitAim == null)
            {
                _superHitAim = FindObjectOfType<SuperHitAim>();
            }
            Assert.IsNotNull(_superHitAim, "_superHitAim != null");
        }

        private void OnDestroy()
        {
            if (_spawner != null)
            {
                _spawner.OnUnitSpawned -= GetTarget;
            }

            if (_targetUnit != null)
            {
                _targetUnit.OnDead -= OnTargetUnitDie;
            }
        }

        private void OnTargetUnitDie()
        {
            _superHitAim.gameObject.SetActive(false);
        }

        private void GetTarget(GameUnit gameUnit)
        {
            if (_currentTargetable != null)
            {
                Debug.LogError($"Targetable already inititalized! - {_currentTargetable.name}");
                return;
            }

            _targetUnit = gameUnit;
            _currentTargetable = gameUnit.GetComponent<Targetable>();
            
            if (_targetUnit != null)
            {
                _targetUnit.OnDead += OnTargetUnitDie;
            }
        }

        public void StartRandomTargeting()
        {
            if (_targetUnit != null && _targetUnit.IsDead)
            {
                return;
            }
            
            if (_currentTargetable == null)
            {
                Debug.LogError("Targetable is missing!");
                return;
            }
            
            if(_superHitAim == null)
                return;

            var randomHit = _currentTargetable.GetRandomHitTarget();
            if (randomHit == null)
            {
                Debug.LogError("HitTarget is missing!");
                return;
            }
            
            _superHitAim.StartTargeting(randomHit);
        }
    }
}

