using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class BattlePlayerControllerBase : MonoBehaviour
    {
        [SerializeField] protected UnitsSpawner _unitsSpawner;
        protected GameUnit _player; 
        protected virtual void Awake()
        {
            if (_unitsSpawner == null)
            { 
                _unitsSpawner = FindObjectOfType<UnitsSpawner>();
            }
            
            Assert.IsNotNull(_unitsSpawner,"_unitsSpawner != null");
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnPlayerSpawn += OnPlayerSpawned;
            }
        }

        protected void OnDestroy()
        {
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnPlayerSpawn -= OnPlayerSpawned;
            }
        }

        protected void OnPlayerSpawned(GameUnit gameUnit)
        {
            _player = gameUnit;
            OnPlayerSpawn();
        }

        protected abstract void OnPlayerSpawn();
    }
}

