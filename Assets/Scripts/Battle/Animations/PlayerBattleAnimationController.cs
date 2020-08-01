using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerBattleAnimationController : UnitAnimationControllerBase
    {

        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(GameHelper.GlobalPlayer, "GameHelper.GlobalPlayer != null");
            if (GameHelper.GlobalPlayer != null)
            {
                PlayerEquipmentController equipmentController = GameHelper.GlobalPlayer.EquipmentController;
                Assert.IsNotNull(equipmentController, "equipmentController != null");

                bool isHaveWeapon = equipmentController.IsHaveEquippedItemOfType(ItemType.EQUIPMENT_WEAPON);
                if (isHaveWeapon)
                {
                    _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_AIM, true);
                }
            }
        }

        protected override void OnAttackTarget(int damage, GameUnit target)
        {
            Debug.LogWarning(new System.NotImplementedException().Message);
        }

        protected override void OnUnitDead()
        {
            Debug.LogWarning(new System.NotImplementedException().Message);
        }

        protected override void OnUnitDamaged(int damage)
        {
            Debug.LogWarning(new System.NotImplementedException().Message);
        }
    }
}
