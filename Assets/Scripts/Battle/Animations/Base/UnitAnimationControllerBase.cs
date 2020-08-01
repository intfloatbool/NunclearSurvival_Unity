using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class UnitAnimationControllerBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        [SerializeField] protected UnitDamage _unitDamage;
        [SerializeField] protected GameUnit _gameUnit;
        
        protected virtual void Awake()
        {
            Assert.IsNotNull(_animator, "_animator != null");
            Assert.IsNotNull(_unitDamage, "_animator != null");
            Assert.IsNotNull(_gameUnit, "_animator != null");

            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged += OnUnitDamaged;
                _gameUnit.OnDead += OnUnitDead;
            }

            if (_unitDamage != null)
            {
                _unitDamage.OnAttackTarget += OnAttackTarget;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged -= OnUnitDamaged;
                _gameUnit.OnDead -= OnUnitDead;
            }

            if (_unitDamage != null)
            {
                _unitDamage.OnAttackTarget -= OnAttackTarget;
            }
        }

        protected abstract void OnAttackTarget(int damage, GameUnit target);

        protected abstract void OnUnitDead();

        protected abstract void OnUnitDamaged(int damage);

    }
}
