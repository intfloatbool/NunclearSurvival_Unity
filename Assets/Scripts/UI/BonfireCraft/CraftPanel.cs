using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using Player;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class CraftPanel : MonoBehaviour, IItemInteractorUI
    {
        [SerializeField] private UiInventory _uiInventory;
        [SerializeField] private InventoryItemCopyUI _itemCopyUIprefab;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private List<InventoryItemCopyUI> _currentItemCopies = new List<InventoryItemCopyUI>();

        private PlayerInventory _playerInventory;
        
        private void Awake()
        {
            _playerInventory = GlobalPlayer.Instance?.PlayerInventory;
            Assert.IsNotNull(_playerInventory, "_playerInventory != null");
            Assert.IsNotNull(_itemContainer, "_itemContainer != null");
            Assert.IsNotNull(_itemCopyUIprefab, "_itemCopyUIprefab != null");
            Assert.IsNotNull(_uiInventory, "_uiInventory != null");
            if (_uiInventory != null)
            {
                _uiInventory.OnItemsUpdated += HandleUpdatedItems;
            }
        }

        private void OnDestroy()
        {
            if (_uiInventory != null)
            {
                _uiInventory.OnItemsUpdated -= HandleUpdatedItems;
            }
        }

        private void OnDisable()
        {
            ClearItemCopies();
        }

        public void ClearItemCopies()
        {
            foreach (var itemCopy in _currentItemCopies)
            {
                Destroy(itemCopy.gameObject);
            }
            
            _currentItemCopies.Clear();
        }

        private int GetItemAmountByNameFromCurrentCopies(ItemName itemName)
        {
            int itemsAmount =
                _currentItemCopies
                    .Count(ic => ic.ItemInfo.ItemName == itemName);
            return itemsAmount;
        }

        private IEnumerable<InventoryItemCopyUI> GetItemCopiesByName(
            ItemName itemName,
            IEnumerable<InventoryItemCopyUI> copyCollection)
        {
            return copyCollection.Where(ic => ic.ItemInfo.ItemName == itemName);
        }

        private void HandleUpdatedItems()
        {
            var currentPlayerItems = _uiInventory.AllItems;
            var itemsCopiesTemp = _currentItemCopies.ToList();
            foreach (var playerItem in currentPlayerItems)
            {
                ItemName itemName = playerItem.CurrentItemInfo.ItemName;
                int itemAmount = playerItem.CurrentAmount;

                int copiesAmount = GetItemAmountByNameFromCurrentCopies(itemName);
                if (copiesAmount > itemAmount)
                {
                    int differenceBetweenAmounts = copiesAmount - itemAmount;
                    int amountIndex = 0;
                    var copiesCollection = GetItemCopiesByName(itemName, itemsCopiesTemp);
                    foreach (var itemCopy in copiesCollection)
                    {
                        if(amountIndex >= differenceBetweenAmounts)
                            break;
                        _currentItemCopies.Remove(itemCopy);
                        Destroy(itemCopy.gameObject);
                        amountIndex++;
                    }
                }
            }
            ClearCopiesFromNulls();
            //check for unexisting items
            foreach (var itemCopy in _currentItemCopies)
            {
                if (currentPlayerItems.Count(pi => pi.CurrentItemInfo.ItemName == itemCopy.ItemInfo.ItemName) <= 0)
                {
                   Destroy(itemCopy.gameObject); 
                }
            }

            ClearCopiesFromNulls();
        }

        private void ClearCopiesFromNulls()
        {
            _currentItemCopies.RemoveAll(ic => ic == null);
        }

        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            if (itemUi == null)
            {
                Debug.LogError("Cannot iteract with null item UI!");
                return;
                
            }
            //TEST
            Debug.Log($"Item {itemUi.CurrentItemInfo.ItemName} dropped in craft panel!");

            CreateCopyOfItem(itemUi);
        }

        public void TryCraft()
        {
            var itemHolder = ItemHolder.Instance;
            if (itemHolder != null)
            {
                var craftSystem = ItemCraftSystem.Instance;
                if (craftSystem == null)
                {
                    Debug.LogError("Craft system is missing!");
                    return;
                }
                //1. Get possible craft items
                var possibleCraftItems = itemHolder.ItemInfos.Where(itemInfo => itemInfo.IsCraftable);
                
                //1.1 Find potential combinations
                var itemCopiesNames = _currentItemCopies.Select(ic => ic.ItemInfo.ItemName)
                    .ToArray();
                foreach (var possibleCraftItemInfo in possibleCraftItems)
                {
                    var itemName = possibleCraftItemInfo.ItemName;
                    var craftedItem = craftSystem.TryCraftItem(itemName, itemCopiesNames);
                    if (craftedItem != null)
                    {
                        _playerInventory.AddItem(craftedItem.ItemName);
                        //remove craft parts from craft menu
                        var craftPartsOfItem = craftedItem.ItemInfo.ItemCraftParts;
                        foreach (var craftPart in craftPartsOfItem)
                        {
                            var craftPartUiCopies =
                                _currentItemCopies.Where(uic => uic.ItemInfo.ItemName == craftPart.ItemPartName).ToList();
                            int counter = 0;
                            foreach (var craftUiCopy in craftPartUiCopies)
                            {
                                if(counter >= craftPart.Amount)
                                    break;
                                _currentItemCopies.Remove(craftUiCopy);
                                Destroy(craftUiCopy.gameObject);
                                
                                counter++;
                            }
                            
                            ClearCopiesFromNulls();
                        }
                    }
                    
                }
            }
        }

        private void CreateCopyOfItem(InventoryItemUi itemUi)
        {
            var amount = itemUi.CurrentAmount;
            var currentCopyAmount = GetItemAmountByNameFromCurrentCopies(itemUi.CurrentItemInfo.ItemName);
            if (currentCopyAmount >= amount)
                return;
            var itemCopy = Instantiate(_itemCopyUIprefab, _itemContainer);
            itemCopy.Init(itemUi.CurrentItemInfo.Clone());
            itemCopy.gameObject.SetActive(true);
            _currentItemCopies.Add(itemCopy);
        }
    }
}
