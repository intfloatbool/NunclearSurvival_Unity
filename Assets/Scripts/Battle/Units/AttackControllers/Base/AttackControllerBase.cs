using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class AttackControllerBase : MonoBehaviour
    {
        [SerializeField] protected bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { this._isEnabled = value; }
        }

        [SerializeField] protected GameUnit _gameUnit;
        [SerializeField] protected UnitDamage _unitDamage;
        
        [SerializeField] protected float _attackDelay = 2f;
        protected bool _isReadyToAttack;
        protected float _attackTimer;
        
        [Space(5f)]
        [Header("RuntimeRefs")]
        [SerializeField] protected GameUnit _currentTarget;
        public GameUnit CurrentTarget
        {
            get { return _currentTarget; }
            set { this._currentTarget = value; }
        }
        
        protected virtual void Awake()
        {
            Assert.IsNotNull(_unitDamage, "_unitDamage != null");
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");
        }

        protected virtual void Update()
        {
            
            if (!_isEnabled)
                return;
            if(_gameUnit == null)
                return;
            if (_unitDamage == null)
                return;
            if (_gameUnit.IsDead)
                return;
            HandleAttack();

            ControlTimeDelay();
        }

        protected void ControlTimeDelay()
        {
            if (_isReadyToAttack)
                return;
            if (_attackTimer >= _attackDelay)
            {
                _isReadyToAttack = true;
                _attackTimer = 0f;
            }
            _attackTimer += Time.deltaTime;
        }

        protected virtual void AttackTarget()
        {
            if (!_isReadyToAttack)
                return;
            if (_currentTarget == null)
                return;
            
            _unitDamage.DamageTargetGameUnit(_currentTarget);
            
            _isReadyToAttack = false;
        }
        protected abstract void HandleAttack();
    }
}
