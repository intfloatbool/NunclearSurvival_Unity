using System;
using UnityEngine;

namespace NunclearGame
{
    public class GameUnit : MonoBehaviour
    {
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
