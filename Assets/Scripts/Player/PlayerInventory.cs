using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerInventory
    {
        [SerializeField] private List<InventoryItem> _currentItems = new List<InventoryItem>();
        public List<InventoryItem> GetCurrentItems() => _currentItems;

        /// <summary>
        /// ItemName -  enum type of item
        /// int - current amount of item
        /// </summary>
        public event Action<ItemName, int> OnItemAdded;
        public event Action<ItemName> OnItemRemoved;

        public event Action OnItemsUpdated;


        public InventoryItem GetItemByName(ItemName itemName)
        {
            return _currentItems.FirstOrDefault(i => i.ItemName == itemName);
        }
        
        public void AddItem(ItemName itemName)
        {
            var sameItem = _currentItems.FirstOrDefault(c => c.ItemName == itemName);
            

            if(sameItem != null)
            {
                var itemInfo = sameItem.ItemInfo;
                if(itemInfo.IsConstantItem)
                {
                    sameItem = new InventoryItem()
                    {
                        ItemName = itemName,
                        Amount = 1
                    };
                    _currentItems.Add(sameItem);
                }
                else
                {
                    sameItem.Amount++;
                }             
            }
            else
            {
                sameItem = new InventoryItem()
                {
                    ItemName = itemName,
                    Amount = 1
                };
                _currentItems.Add(sameItem);
            }

            OnItemAdded?.Invoke(sameItem.ItemName, sameItem.Amount);
            OnItemsUpdated?.Invoke();
        }

        public void RemoveItem(ItemName itemName)
        {
            var sameItem = _currentItems.FirstOrDefault(c => c.ItemName == itemName);
            if (sameItem != null)
            {
                sameItem.Amount--;
                if (sameItem.Amount <= 0)
                {
                    _currentItems.Remove(sameItem);
                }

                OnItemRemoved?.Invoke(itemName);
                OnItemsUpdated?.Invoke();

            }
            else
            {
                Debug.LogError($"Cannot delete item from player inventory with key {itemName} ! Not exist!");
            }
        }
    }

}
