using System;
using Common.Values;
using UnityEngine;

namespace NunclearGame.Battle
{
    public class GameUnit : MonoBehaviour, INormalizedValueProvider
    {
        [SerializeField] protected string _nameKey;
        public string NameKey => _nameKey;
        
        [SerializeField] protected int _currentHp;
        protected int CurrentHp => _currentHp;
        protected int _maxHp;
        public float HpPercent => 
            (float) _currentHp / (float) _maxHp;

        [SerializeField] protected bool _isDead;
        public bool IsDead => _isDead;
        
        public event Action<int> OnDamaged;
        public event Action OnDead;

        protected virtual void Awake()
        {
            _maxHp = _currentHp;
        }
        
        public float GetNormalizedValue()
        {
            return HpPercent;
        }

        public virtual void MakeDamage(int dmg)
        {
            if (_isDead)
            {
                return;
            }
            
            _currentHp -= dmg;
            OnDamaged?.Invoke(dmg);
            if (_currentHp <= 0)
            {
                _isDead = true;
                _currentHp = 0;
                OnDead?.Invoke();
            }
        }

    }
}
