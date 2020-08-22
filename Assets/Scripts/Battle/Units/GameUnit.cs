using System;
using Common.Values;
using UnityEngine;

namespace NunclearGame.Battle
{
    public class GameUnit : MonoBehaviour, INormalizedValueProvider
    {
        //DEBUG
        [Header("DEBUG")] 
        [SerializeField] private bool _isImmortal;
        
        [Space(5f)]
        [SerializeField] protected string _nameKey;
        public string NameKey => _nameKey;
        
        [SerializeField] protected int _currentHp;
        protected int CurrentHp => _currentHp;
        protected int _maxHp;
        public float HpPercent => 
            (float) _currentHp / (float) _maxHp;

        [SerializeField] protected bool _isDead;
        public bool IsDead => _isDead;

        [Space(5f)]
        [SerializeField] protected bool _isStunnable;
        [ConditionalHide("_isStunnable")]
        [SerializeField] protected float _stunTime = 1.5f;
        [Space(2f)]
        [Header("Runtime")]
        [ConditionalHide("_isStunnable")]
        [SerializeField] protected bool _isStunnedNow;

        public bool IsStunnedNow
        {
            get { return _isStunnedNow; }
        }
        protected float _stunTimer;

        public bool IsInvulnerability { get; set; } = false;

        public event Action<int> OnDamaged;

        public event Action OnDead;
        public event Action<GameUnit> OnDeadWithRef;

        public event Action OnUnitStunned;

        protected virtual void Awake()
        {
            _maxHp = _currentHp;
        }

        public void MakeStun()
        {
            if (_isStunnable)
            {
                _isStunnedNow = true;
                OnUnitStunned?.Invoke();
            }
        }
        
        public float GetNormalizedValue()
        {
            return HpPercent;
        }

        public virtual void MakeDamage(int dmg)
        {
            if (_isImmortal)
                return;
            
            if(IsInvulnerability)
                return;
            
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
                
                Debug.Log($"{this.gameObject.name} is DEAD!");
                OnDeadWithRef?.Invoke(this);
                OnDead?.Invoke();
            }
        }

        protected virtual void Update()
        {
            if (_isStunnable)
                HandleStun();
        }

        protected virtual void HandleStun()
        {
            if (!_isStunnedNow)
                return;
            if (_stunTimer >= _stunTime)
            {
                _isStunnedNow = false;
                _stunTimer = 0;
            }

            _stunTimer += Time.deltaTime;
        }
    }
}
