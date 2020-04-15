using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class BonfireCookerUI : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private UiInventory _inventoryUi;
        [SerializeField] private Transform _inputItemsParent;
        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            _inventoryUi.RemoveItem(itemUi.CurrentItemInfo);
            itemUi.transform.SetParent(_inputItemsParent);
        }
        
    }
}

