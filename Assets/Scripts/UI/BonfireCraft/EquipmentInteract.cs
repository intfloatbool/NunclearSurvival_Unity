using GameUI;
using SingletonsPreloaders;
using UnityEngine;

namespace NunclearGame.BonfireSceneUI
{
    public class EquipmentInteract : MonoBehaviour, IItemInteractorUI
    {
        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            Debug.Log($"Try equip item: {itemUi.CurrentItemInfo.ItemName}");
            if (!GlobalPlayer.PlayerEquipmentController.IsEquipable(itemUi.CurrentItemInfo))
            {
                return;
            }
            var itemInfoClone = itemUi.CurrentItemInfo.Clone();
            GlobalPlayer.PlayerEquipmentController.Equip(itemInfoClone);
            Debug.Log($"{itemInfoClone.ItemName} equipped as {itemInfoClone.ItemType}!");
        }
    }
}

