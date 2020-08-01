using UnityEngine;

namespace NunclearGame.Battle
{
    public class PlayerTapAttackController : AttackControllerBase
    {
        protected override void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackTarget();
            }
        }
    }
}
