using NunclearGame.Static;

namespace NunclearGame.Battle
{
    public class StandartUnitAnimationController : UnitAnimationControllerBase
    {
        protected override void OnAttackTarget()
        {
            string randomAttackTrigger = GameHelper.AnimationKeys.DefaultAnimationsKeys.GetRndAttackTriggerKey();
            _animator.Play(GameHelper.AnimationKeys.DefaultAnimationsKeys.GetAttackClipByTrigger(randomAttackTrigger));
            _animator.SetTrigger(randomAttackTrigger);
        }

        protected override void OnUnitDead()
        {
            _animator.Play(GameHelper.AnimationKeys.DefaultAnimationsKeys.ANIM_DEATH);
            _animator.SetBool(GameHelper.AnimationKeys.DefaultAnimationsKeys.IS_DEAD_BOOL, true);
        }

        protected override void OnUnitDamaged(int damage)
        {
            _animator.Play(GameHelper.AnimationKeys.DefaultAnimationsKeys.ANIM_HIT);
            _animator.SetTrigger(GameHelper.AnimationKeys.DefaultAnimationsKeys.HIT_TRIGGER);
        }
    }
}

