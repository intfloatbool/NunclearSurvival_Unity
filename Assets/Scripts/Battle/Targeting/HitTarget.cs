using UnityEngine;

namespace NunclearGame.Battle
{
    public class HitTarget : MonoBehaviour
    {
        private Targetable _targetable;

        public void Init(Targetable targetable)
        {
            _targetable = targetable;
        }

        public void Affect()
        {
            if (_targetable != null)
            {
                _targetable.MakeEffectToTarget(this);
            }
        }
    }

}
