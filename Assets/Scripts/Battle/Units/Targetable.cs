using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace NunclearGame.Battle
{
    public class Targetable : MonoBehaviour
    {
        [SerializeField] private Transform _hitEffect;
        private HitTarget[] _targets;
        
        public HitTarget LastAffectedTarget { get; private set; }
        
        private void Awake()
        {
            _targets = GetComponentsInChildren<HitTarget>();
            Assert.IsTrue(_targets.Length > 0, "_targets.Length > 0");

            if (_targets != null && _targets.Length > 0)
            {
                InitAllTargets();
            }
        }

        private void InitAllTargets()
        {
            for (int i = 0; i < _targets.Length; i++)
            {
                var targetInArr = _targets[i];
                if (targetInArr != null)
                {
                    targetInArr.Init(this);
                } 
            }
        }

        public HitTarget GetRandomHitTarget()
        {
            if (_targets.Length > 0)
            {
                return _targets[Random.Range(0, _targets.Length)];
            }

            return null;
        }

        public void MakeEffectToTarget(HitTarget target)
        {
            if (_hitEffect == null)
                return;
            
            for (int i = 0; i < _targets.Length; i++)
            {
                var targetInArr = _targets[i];
                if (targetInArr != null && targetInArr == target)
                {
                    _hitEffect.transform.position = target.transform.position;
                    _hitEffect.gameObject.SetActive(true);
                    LastAffectedTarget = target;
                } 
            }
        }
    } 
}

