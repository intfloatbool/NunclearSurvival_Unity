using UnityEngine;

namespace NunclearGame.Battle
{
    public class BotAttackController : AttackControllerBase
    {
        [SerializeField] private bool _isDelay = true;
        protected override void HandleAttack()
        {
            if (_isDelay)
            {
                AttackTargetWithDelay();   
            }
            else
            {
                AttackTarget();    
            }
        }
    }
}

