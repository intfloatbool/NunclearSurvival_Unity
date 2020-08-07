using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class UnitAnimationControllerBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AttackControllerBase _attackController;
        [SerializeField] protected GameUnit _gameUnit;
        
        protected virtual void Awake()
        {
            Assert.IsNotNull(_animator, "_animator != null");
            Assert.IsNotNull(_gameUnit, "_animator != null");

            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged += OnUnitDamaged;
                _gameUnit.OnDead += OnUnitDead;
            }

            if (_attackController != null)
            {
                _attackController.OnAttackStarted += OnAttackTarget;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged -= OnUnitDamaged;
                _gameUnit.OnDead -= OnUnitDead;
            }

            if (_attackController != null)
            {
                _attackController.OnAttackStarted -= OnAttackTarget;
            }
        }

        protected abstract void OnAttackTarget();

        protected abstract void OnUnitDead();

        protected abstract void OnUnitDamaged(int damage);

    }
}
