﻿using System;
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
        
        [SerializeField] protected UnitDamage _unitDamage;
        
        [SerializeField] protected GameUnit _currentTarget;

        public GameUnit CurrentTarget
        {
            get { return _currentTarget; }
            set { this._currentTarget = value; }
        }
        
        [SerializeField] protected float _attackDelay = 2f;
        protected bool _isReadyToAttack;
        protected float _attackTimer;

        protected virtual void Awake()
        {
            Assert.IsNotNull(_unitDamage, "_unitDamage != null");
        }

        protected virtual void Update()
        {
            if (!_isEnabled)
                return;
            if (_unitDamage == null)
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
            
            if (_currentTarget != null)
            {
                _unitDamage.DamageTargetGameUnit(_currentTarget);
            }
            _isReadyToAttack = false;
        }
        protected abstract void HandleAttack();
    }
}
