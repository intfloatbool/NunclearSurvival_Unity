using NunclearGame.Static;

namespace NunclearGame.Battle
{
    public class StandartUnitAnimationController : UnitAnimationControllerBase
    {
        protected override void OnAttackTarget(int damage, GameUnit target)
        {
            string randomAttackTrigger = GameHelper.AnimationKeys.DefaultAnimationsKeys.GetRndAttackTriggerKey();
            _animator.SetTrigger(randomAttackTrigger);
        }

        protected override void OnUnitDead()
        {
            _animator.SetBool(GameHelper.AnimationKeys.DefaultAnimationsKeys.IS_DEAD_BOOL, true);
        }

        protected override void OnUnitDamaged(int damage)
        {
            _animator.SetTrigger(GameHelper.AnimationKeys.DefaultAnimationsKeys.HIT_TRIGGER);
        }
    }
}

