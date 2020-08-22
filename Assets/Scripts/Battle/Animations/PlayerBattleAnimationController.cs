using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerBattleAnimationController : UnitAnimationControllerBase
    {
        private bool _isHaveWeapon;

        private BattleResultController _resultController;
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

            _resultController = FindObjectOfType<BattleResultController>();
            Assert.IsNotNull(_resultController, "_resultController != null");
            if (_resultController != null)
            {
                _resultController.OnBattleWin += OnBattleWin;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_resultController != null)
            {
                _resultController.OnBattleWin -= OnBattleWin;
            }
        }

        private void OnBattleWin()
        {
            if (_isHaveWeapon)
            {
                _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_AIM, false);
            }
        }

        protected override void OnAttackTarget()
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
        

        protected override void OnUnitStunned()
        {
            Debug.LogWarning("Player cannot be stunned?");
        }
    }
}
