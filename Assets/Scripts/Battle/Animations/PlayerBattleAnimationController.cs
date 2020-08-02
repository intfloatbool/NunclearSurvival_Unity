using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerBattleAnimationController : UnitAnimationControllerBase
    {
        private bool _isHaveWeapon;
        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(GameHelper.GlobalPlayer, "GameHelper.GlobalPlayer != null");
            if (GameHelper.GlobalPlayer != null)
            {
                PlayerEquipmentController equipmentController = GameHelper.GlobalPlayer.EquipmentController;
                Assert.IsNotNull(equipmentController, "equipmentController != null");

                _isHaveWeapon = equipmentController.IsHaveEquippedItemOfType(ItemType.EQUIPMENT_WEAPON);
                if (_isHaveWeapon)
                {
                    _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_AIM, true);
                }
            }
        }

        protected override void OnAttackTarget(int damage, GameUnit target)
        {
            if (!_isHaveWeapon)
            {
                _animator.SetTrigger(GameHelper.AnimationKeys.PlayerAnimationKeys.HAND_PUNCH_TRIGGER);
            }
        }

        protected override void OnUnitDead()
        {
            _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_DEAD_BOOL, true);
        }

        protected override void OnUnitDamaged(int damage)
        {
            
        }
    }
}
