using SingletonsPreloaders;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class BonfireCookerUI : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private string _onlyFoodAttentionLocKey = "onlyFoodAttentionKey";

        [SerializeField] private UiInventory _inventoryUi;
        [SerializeField] private Transform _inputItemsParent;
        [SerializeField] private List<InventoryItemUi> _currentItems;
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
           
            AddItemToBonfire(itemUi);
        }

        private void AddItemToBonfire(InventoryItemUi itemUi) 
        {
            var sameItem = _currentItems.FirstOrDefault( i => i.CurrentItemInfo.ItemName == itemUi.CurrentItemInfo.ItemName );
            if(sameItem != null)
            {
                sameItem.CurrentAmount++;
            }
            else 
            {
                var clone = itemUi.Clone(_inputItemsParent);
                clone.ExternalOnClickAction = OnItemClick;
                clone.CurrentAmount = 1;
                _currentItems.Add(clone);              
            }  
            _inventoryUi.RemoveItem(itemUi.CurrentItemInfo);          
        }

        private void OnItemClick(InventoryItemUi itemUi)
        {
            var globalPlayer  = GlobalPlayer.Instance;
            if(globalPlayer != null)
            {
                var inventory = globalPlayer.PlayerInventory;
                inventory.AddItem(itemUi.CurrentItemInfo.ItemName);
            }            
            if(itemUi.CurrentAmount > 1) 
            {
                itemUi.CurrentAmount--;
            }
            else 
            {
                _currentItems.Remove(itemUi);
                Destroy(itemUi.gameObject);
            }
            
            _inventoryUi.UpdateLastCategories();
           
        }
    }
}

