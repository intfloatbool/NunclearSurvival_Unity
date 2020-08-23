using System;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerViewAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_playerView == null)
            {
                _playerView = GetComponent<PlayerView>();
            }

            Assert.IsNotNull(_playerView, "_playerView != null");

            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

            Assert.IsNotNull(_animator, "_animator != null");

            if (_playerView != null)
            {
                _playerView.OnItemViewEquipped += PlayerItemEquipped;
                _playerView.OnItemUnequipped += PlayerItemUnequipped;
            }

            var equipmentController = GameHelper.GlobalPlayer.EquipmentController;

            var isHaveWeapon = equipmentController.IsHaveEquippedItemOfType(ItemType.EQUIPMENT_WEAPON);

            if (isHaveWeapon)
            {
                _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_PLAYER_HAVE_WEAPON, true);
            }
        }

        private void OnDestroy()
        {
            if (_playerView != null)
            {
                _playerView.OnItemViewEquipped -= PlayerItemEquipped;
                _playerView.OnItemUnequipped -= PlayerItemUnequipped;
            }
        }

        private void PlayerItemEquipped(ItemType itemType)
        {
            if (_animator == null)
                return;

            if (itemType == ItemType.EQUIPMENT_WEAPON)
            {
                _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_PLAYER_HAVE_WEAPON, true);
            }
        }

        private void PlayerItemUnequipped(ItemType itemType)
        {
            if (itemType == ItemType.EQUIPMENT_WEAPON)
            {
                _animator.SetBool(GameHelper.AnimationKeys.PlayerAnimationKeys.IS_PLAYER_HAVE_WEAPON, false);
            }
        }
    }
}