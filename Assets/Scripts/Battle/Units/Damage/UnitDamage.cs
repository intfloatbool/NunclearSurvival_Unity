using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

namespace NunclearGame.Battle
{
    public class UnitDamage : MonoBehaviour
    {
        [ConditionalHide("_isRandomDamage", Inverse = true)]
        [SerializeField] protected int _damage = 10;
        
        [SerializeField] protected bool _isRandomDamage;

        [ConditionalHide("_isRandomDamage")] 
        [SerializeField] protected int _minDamage = 8;
        [ConditionalHide("_isRandomDamage")] 
        [SerializeField] protected int _maxDamage = 10;
        
        [Range(0, 1f)]
        [SerializeField] protected float _criticalChance = 0.3f;
        
        [SerializeField] protected float _criticalMultipler = 1.5f;
        
        /// <summary>
        /// arg#0 (Int) - damage of attack
        /// arg#1 (GameUnit) - target of attack
        /// </summary>
        public event Action<int, GameUnit> OnAttackTarget;

        protected virtual void Awake()
        {
            if (_isRandomDamage)
            {
                Assert.IsTrue(_maxDamage > _minDamage, "_maxDamage > _minDamage");
            }
        }

        private bool IsCritical()
        {
            int randomValue = Random.Range(0, 100);
            int critChance = Mathf.RoundToInt(_criticalChance * 10f);
            if (randomValue <= critChance)
            {
                return true;
            }

            return false;
        }

        public void DamageTargetGameUnit(GameUnit gameUnit)
        {
            if (gameUnit == null)
            {
                Debug.LogError($"Target {nameof(GameUnit)} is null!");
            }
            int damage = _damage;
            if (_isRandomDamage)
            {
                damage = Random.Range(_minDamage, _maxDamage);
            }
            bool isCritical = IsCritical();
            if (isCritical)
            {
                Debug.Log($"Critical damage by {gameObject.name}!");
                damage = (int) (damage * _criticalMultipler);
            }
            
            gameUnit.MakeDamage(damage);
            OnAttackTarget?.Invoke(damage, gameUnit);
        }

    }
}
