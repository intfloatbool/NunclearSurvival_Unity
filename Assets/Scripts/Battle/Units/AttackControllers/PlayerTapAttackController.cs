using System;
using NunclearGame.Battle.UI;
using NunclearGame.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerTapAttackController : AttackControllerBase
    {
        [SerializeField] private PlayerView _playerView;
        
        [SerializeField] private float _superHitDamageMultipler = 2f;
        public float SuperHitDamageMultipler
        {
            get => _superHitDamageMultipler;
            set => _superHitDamageMultipler = value;
        }
        
        [SerializeField] private float _timeToShowAim = 1.5f;
        
        private SuperHitController _superHitController;
        private SuperHitAim _superHitAim;
        private WeaponEquipViewItem _weaponEquipViewItem;
        
        private float _showAimTimer;
        private WeaponView _currentWeaponView;

        private Targetable _targetable;
        private Targetable Targetable
        {
            get
            {
                if (_targetable == null)
                {
                    _targetable = _currentTarget.GetComponent<Targetable>();
                }

                return _targetable;
            }
        }

        public HitTarget LastHitTarget { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            if (_playerView == null)
            {
                _playerView = FindObjectOfType<PlayerView>();    
            }
            Assert.IsNotNull(_playerView, "_playerView != null");
            
            _superHitController = FindObjectOfType<SuperHitController>();
            Assert.IsNotNull(_superHitController, "_superHitController != null");

            _superHitAim = _superHitController?.SuperHitAim;
            Assert.IsNotNull(_superHitAim);
            if (_superHitAim != null)
            {
                _superHitAim.OnAimDoneTargeting += CorrectHitDamageByAim;
            }
        }

        private void Start()
        {
            if (_playerView != null)
            {
                _weaponEquipViewItem = _playerView.GetViewItemByType(ItemType.EQUIPMENT_WEAPON) as WeaponEquipViewItem;
                if (_weaponEquipViewItem != null)
                {
                    _weaponEquipViewItem.OnWeaponViewChanged += SetWeaponItem;
                }
            }
        }
        
        private void OnDestroy()
        {
            if (_superHitAim != null)
            {
                _superHitAim.OnAimDoneTargeting -= CorrectHitDamageByAim;
            }

            if (_weaponEquipViewItem != null)
            {
                _weaponEquipViewItem.OnWeaponViewChanged -= SetWeaponItem;
            }
        }
        
        private void SetWeaponItem(WeaponView weaponView)
        {
            _currentWeaponView = weaponView;
        }

        private void CorrectHitDamageByAim(HitTarget hitTarget, bool isAimDone)
        {
            if (isAimDone)
            {
                _unitDamage.ExternalCritMultipler = _superHitDamageMultipler;
            }

            LastHitTarget = hitTarget;
            AttackTargetForce();
        }
        
        private void ResestTimer()
        {
            _showAimTimer = 0f;
        }

        protected override void AttackTarget()
        {
            if (!_isReadyToAttack)
                return;
            if (_currentTarget == null)
                return;
            
            AttackTargetForce();
        }

        private void AttackTargetForce()
        {
            if (_currentTarget == null || _currentTarget.IsDead)
                return;

            ShowAttackEffects();
            base.AttackTarget();
        }

        private void ShowAttackEffects()
        {
            if (_currentWeaponView != null)
            {
                _currentWeaponView.WeaponEffectsShow();
            }

            if (LastHitTarget == null)
            {
                LastHitTarget = Targetable?.GetRandomHitTarget();
            }
            
            if (LastHitTarget != null)
            {
                LastHitTarget.Affect();
                LastHitTarget = null;
            }
        }

        protected override void HandleAttack()
        {
            if (_superHitController.IsAimInProcess)
            {
                ResestTimer();
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                AttackTarget();
                ResestTimer();
            }
            
            
            if (_showAimTimer >= _timeToShowAim)
            {
                _superHitController.StartRandomTargeting();
            }
            _showAimTimer += Time.deltaTime;
        }
    }
}
