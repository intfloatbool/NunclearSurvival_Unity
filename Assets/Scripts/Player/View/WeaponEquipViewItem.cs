using System;
using NunclearGame.Battle;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class WeaponEquipViewItem : EquipViewItem
    {
        public WeaponView WeaponView { get; private set; }
        public event Action<WeaponView> OnWeaponViewChanged;
        public override void SetItem(GameObject itemPrefab)
        {
            base.SetItem(itemPrefab);

            if (_currentItemInstance != null)
            {
                WeaponView = _currentItemInstance.GetComponent<WeaponView>();
                Assert.IsNotNull(WeaponView, "WeaponView != null");
                if (WeaponView != null)
                {
                    OnWeaponViewChanged?.Invoke(WeaponView);
                }
            }
        }
    }
}

