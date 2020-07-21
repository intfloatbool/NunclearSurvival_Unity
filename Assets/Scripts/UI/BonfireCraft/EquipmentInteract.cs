using System;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using NunclearGame.Player;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class EquipmentInteract : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private List<EquipItemUi> _equipmentItemsCollection;

        private EquipItemUi GetEquipItemUiByType(ItemType itemType)
        {
            return _equipmentItemsCollection.FirstOrDefault(itemUi => itemUi.ItemType == itemType);
        }

        private void Awake()
        {
            Assert.IsTrue(_equipmentItemsCollection.Any(), "_equipmentItemsCollection.Any()");
            
            UpdateEquipmentUi(GlobalPlayer.PlayerEquipmentController.CurrentEquipment);
            GlobalPlayer.PlayerEquipmentController.OnEquipmentChanged += UpdateEquipmentUi;
        }

        private void OnDestroy()
        {
            GlobalPlayer.PlayerEquipmentController.OnEquipmentChanged -= UpdateEquipmentUi;
        }

        private void UpdateEquipmentUi(PlayerEquipment playerEquipment)
        {
            foreach (EquipmentValue equipValue in GlobalPlayer.PlayerEquipmentController.TotalEquipmentCollection)
            {
                var equipUI = GetEquipItemUiByType(equipValue.ItemType);
                if (equipUI != null)
                {
                    ItemName itemName = equipValue.ItemName;
                    if (itemName == ItemName.NONE)
                    {
                        equipUI.SetActive(false);
                        continue;
                    }
                    ItemInfo itemInfo = ItemHolder.Instance.GetItemInfoByKey(itemName);
                    if (itemInfo != null)
                    {
                        equipUI.InitItem(itemInfo.ItemIcon);
                        equipUI.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError($"There is no ItemInfo in itemHolder with name: {itemName}!");
                    }
                    
                }
                else
                {
                    Debug.LogError($"Equip UI for type {equipValue.ItemType} not found!");
                }
            }
        }

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

