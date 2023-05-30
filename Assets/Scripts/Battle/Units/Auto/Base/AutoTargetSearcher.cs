using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class AutoDamageTargetSearcherBase : MonoBehaviour
    {
        [SerializeField] protected AttackControllerBase _attackController;
        [SerializeField] protected float _searchingLoopDelay = 1f;

        protected WaitForSeconds _waiting;
        protected abstract string TargetTag { get; }
        
        protected virtual void Awake()
        {
            Assert.IsNotNull(_attackController, "_attackController != null");
        }

        private IEnumerator Start()
        {
            if(_attackController == null)
                yield break;
            
            _waiting = new WaitForSeconds(_searchingLoopDelay);
            
            //target searching loop
            while (_attackController.CurrentTarget == null)
            {
                yield return _waiting;
                GameObject targetGo = GameObject.FindWithTag(TargetTag);
                if (targetGo != null)
                {
                    GameUnit gameUnit = targetGo.GetComponent<GameUnit>();
                    if (gameUnit != null)
                    {
                        _attackController.CurrentTarget = gameUnit;
                    }
                }
            }
        }
    }
}

