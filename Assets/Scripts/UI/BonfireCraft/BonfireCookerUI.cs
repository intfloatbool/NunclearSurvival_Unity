using SingletonsPreloaders;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class BonfireCookerUI : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private string _onlyFoodOrCookItemAttentionLocKey = "onlyFoodOrCookAttentionKey";

        [SerializeField] private UiInventory _inventoryUi;
        [SerializeField] private Transform _inputItemsParent;
        [SerializeField] private List<InventoryItemUi> _currentItems;
        [SerializeField] private InventoryItemUi _currentCookItem;
        private void Start()
        {
            Debug.Assert(_inventoryUi != null, "_inventoryUi != null");
            Debug.Assert(_inputItemsParent != null, "_inputItemsParent != null");
        }

        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            var itemType = itemUi.CurrentItemInfo.ItemType;
            if(itemType != ItemType.FOOD || itemType != ItemType.COOK_ITEM)
            {
                var guiDialog = CommonGui.Instance?.GetDialog();
                if(guiDialog != null)
                {
                    guiDialog.ShowAttentionDialog(_onlyFoodOrCookItemAttentionLocKey);
                }

                return;
            }

            AddItemToBonfire(itemUi);
        }


        private void UpdateCookItem(InventoryItemUi itemUi) {

        }

        private void AddItemToBonfire(InventoryItemUi itemUi) 
        {
            if(itemUi.CurrentItemInfo.ItemType == ItemType.COOK_ITEM)
            {
                if(_currentCookItem != null)
                {
                    _currentCookItem.ItemSprite = itemUi.ItemSprite;
                }
                else
                {
                    //TODO: logic for updating new cook item
                }
            }
            else 
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
                   
        }

        private void OnItemClick(InventoryItemUi itemUi)
        {
            if(itemUi.CurrentItemInfo.ItemType == ItemType.COOK_ITEM) 
            {
                return;
            }
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

