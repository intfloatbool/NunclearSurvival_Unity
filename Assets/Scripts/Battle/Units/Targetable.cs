using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace NunclearGame.Battle
{
    public class Targetable : MonoBehaviour
    {
        private HitTarget[] _targets;

        private void Awake()
        {
            _targets = GetComponentsInChildren<HitTarget>();
            Assert.IsTrue(_targets.Length > 0, "_targets.Length > 0");
        }

        public HitTarget GetRandomHitTarget()
        {
            if (_targets.Length > 0)
            {
                return _targets[Random.Range(0, _targets.Length)];
            }

            return null;
        }
    } 
}

