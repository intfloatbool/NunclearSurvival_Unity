using System;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class BattleResultController : MonoBehaviour
    {
        [SerializeField] private UnitsSpawner _unitsSpawner;

        private GameUnit _playerUnit;
        private GameUnit _enemyUnit;

        public event Action OnBattleWin;
        public event Action OnBattleFail;
        
        private void Awake()
        {
            Assert.IsNotNull(_unitsSpawner, "_unitsSpawner != null");
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned += OnUnitSpawned;
                _unitsSpawner.OnSpawnDone += CheckAllRefs;
            }
        }

        private void OnDestroy()
        {
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned -= OnUnitSpawned;
                _unitsSpawner.OnSpawnDone -= CheckAllRefs;
            }

            if (_enemyUnit != null)
            {
                _enemyUnit.OnDeadWithRef -= OnUnitDead;
            }

            if (_playerUnit != null)
            {
                _playerUnit.OnDeadWithRef -= OnUnitDead;
            }
        }

        private void CheckAllRefs()
        {
            Assert.IsNotNull(_enemyUnit, "_enemyUnit != null");
            Assert.IsNotNull(_playerUnit, "_playerUnit != null");
        }

        private void OnUnitDead(GameUnit unit)
        {
            if (unit != _enemyUnit && unit != _playerUnit)
            {
                Debug.LogError($"{unit.name} This unit not initialized!");
                return;
            }

            //TODO: COmplete battle end LOGIC
            if (unit == _playerUnit)
            {
                _enemyUnit.IsInvulnerability = true;
                Debug.Log("PLAYER LOSE METRO LEVEL");
                OnBattleFail?.Invoke();
            }
            else if (unit == _enemyUnit)
            {
                _playerUnit.IsInvulnerability = true;
                Debug.Log("PLAYER WIN METRO LEVEL!");
                OnBattleWin?.Invoke();
            }
        }

        private void OnUnitSpawned(GameUnit gameUnit)
        {
            if (gameUnit.tag.Equals(GameHelper.GameTags.PLAYER_TAG))
            {
                _playerUnit = gameUnit;
            }
            else if (gameUnit.tag.Equals(GameHelper.GameTags.METRO_ENEMY_TAG))
            {
                _enemyUnit = gameUnit;
            }
            else
            {
                return;
            }

            gameUnit.OnDeadWithRef += OnUnitDead;


        }
    }
}
