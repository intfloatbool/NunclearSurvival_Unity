using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerViewAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private Animator _animator;

        [SerializeField] private string _isHaveWeaponAnimKey = "IsHaveWeapon";
        
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
                _animator.SetBool(_isHaveWeaponAnimKey, true);
            }
            
        }

        private void PlayerItemUnequipped(ItemType itemType)
        {
            if (itemType == ItemType.EQUIPMENT_WEAPON)
            {
                _animator.SetBool(_isHaveWeaponAnimKey, false);
            }
        }
    }
}
