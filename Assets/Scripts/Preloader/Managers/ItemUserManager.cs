using NunclearGame.Static;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class ItemUserManager : UnitySingletonBase<ItemUserManager>
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
                        // GlobalPlayer.PlayerValuesController.HealUp(hungryHealValue.Value);
                        GlobalPlayer.PlayerValuesController.ChangeHealth(hungryHealValue.Value);
                    }
                    GlobalPlayer.Inventory.RemoveItem(itemInfo.ItemName);
                    break;
                }
                case ItemType.EQUIPMENT_WEAPON:
                {
                    GlobalPlayer.PlayerEquipmentController.Equip(itemInfo);
                    break;
                }
                case ItemType.EQUIPMENT_ARMOR:
                {
                    GlobalPlayer.PlayerEquipmentController.Equip(itemInfo);
                    break;
                }
                case ItemType.EQUIPMENT_GRENADE:
                {
                    GlobalPlayer.PlayerEquipmentController.Equip(itemInfo);
                    break;
                }
            }
        }
    }
}

