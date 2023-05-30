using Common.Dependencies;
using NunclearGame.Static;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class ItemUserManager : UnitySingletonBase<ItemUserManager>, ISingletonDependency
    {
        protected override ItemUserManager GetInstance() => this;

        public void UseItem(ItemInfo itemInfo)
        {
            //TODO: Complete item using
            ItemName itemName = itemInfo.ItemName;
            ItemType itemType = itemInfo.ItemType;
            switch (itemType)
            {
                case ItemType.FOOD:
                {
                    int? hungryHealValue = itemInfo.GetItemValueByKey(GameHelper.ItemValueKeys.FOOD_NUTRITIONAL);
                    if (hungryHealValue != null)
                    {
                        GlobalPlayer.PlayerValuesController.AddHealth(hungryHealValue.Value);
                    }
                    GlobalPlayer.Inventory.RemoveItem(itemInfo.ItemName);
                    break;
                }
                case ItemType.ENERGETIC:
                {
                    int? staminaRecoveryValue = itemInfo.GetItemValueByKey(GameHelper.ItemValueKeys.STAMINA_RECOVERY);
                    if (staminaRecoveryValue != null)
                    {
                        GlobalPlayer.PlayerValuesController.AddStamina(staminaRecoveryValue.Value);
                    }
                    GlobalPlayer.Inventory.RemoveItem(itemInfo.ItemName);
                    break;
                }
                case ItemType.EQUIPMENT_WEAPON:
                case ItemType.EQUIPMENT_ARMOR:
                case ItemType.EQUIPMENT_GRENADE:
                {
                    GlobalPlayer.PlayerEquipmentController.Equip(itemInfo);
                    break;
                }
            }
        }

        public void SelfRegister()
        {
            DepResolver.RegisterDependency(this);
        }
    }
}

