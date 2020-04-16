using SingletonsPreloaders;
using StaticHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class BonfireCookerUI : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private string _onlyFoodAttentionLocKey = "onlyFoodAttentionKey";

        [SerializeField] private UiInventory _inventoryUi;
        [SerializeField] private Transform _inputItemsParent;

        private void Start()
        {
            Debug.Assert(_inventoryUi != null, "_inventoryUi != null");
            Debug.Assert(_inputItemsParent != null, "_inputItemsParent != null");
        }

        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            var itemType = itemUi.CurrentItemInfo.ItemType;
            if(itemType != ItemType.FOOD)
            {
                var guiDialog = CommonGui.Instance?.GetDialog();
                if(guiDialog != null)
                {
                    guiDialog.ShowAttentionDialog(_onlyFoodAttentionLocKey);
                }

                return;
            }
           
            var clone = itemUi.Clone(_inputItemsParent);
            clone.ExternalOnClickAction = OnItemClick;

            _inventoryUi.RemoveItem(itemUi.CurrentItemInfo);

        }

        private void OnItemClick(InventoryItemUi itemUi)
        {
            _inventoryUi.AddItem(itemUi.CurrentItemInfo, itemUi.CurrentAmount);
            Destroy(itemUi.gameObject);
        }
    }
}

