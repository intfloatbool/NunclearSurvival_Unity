using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerUnitDamage : UnitDamage
    {
        protected override void Awake()
        {
            Assert.IsNotNull(GameHelper.GlobalPlayer, "GlobalPlayer != null");
            if (GameHelper.GlobalPlayer != null)
            {
                ItemInfo currentWeapon =
                    GameHelper.GlobalPlayer.GetItemInfoFromCurrentEquipmentByType(ItemType.EQUIPMENT_WEAPON);
                if (currentWeapon != null)
                {
                    int? weaponValue = currentWeapon.GetItemValueByKey(GameHelper.ItemValueKeys.WEAPON_DAMAGE);
                    if (weaponValue != null)
                    {
                        _damage = weaponValue.Value;
                    }
                    else
                    {
                        Debug.LogError($"Weapon of type {currentWeapon.ItemName} don't have damage param!");
                    }
                }
            }
            
        }
    }
}

