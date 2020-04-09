using System.Collections.Generic;
using Player;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class ItemCraftSystem : UnitySingletonBase<ItemCraftSystem>
    {
        [Header("Debug tests")] 
        [SerializeField] private ItemName _itemToCraftDebug;

        [ContextMenu("Craft item by type debug")]
        public void CraftItemDebug()
        {
            var craftedItem = TryCraftItem(_itemToCraftDebug);
            Debug.Log("DEBUG: item crafted: ");
            if (craftedItem == null)
            {
                Debug.LogError("Cannot craft item!");
            }
            else
            {
                Debug.Log($"Crafted item: {craftedItem.ItemName}, amount: {craftedItem.Amount}");
                GlobalPlayer.Instance.PlayerInventory.AddItem(_itemToCraftDebug);
            }
        }
        
        protected override ItemCraftSystem GetInstance() => this;
        
        public InventoryItem TryCraftItem(ItemName sourceItemName)
        {
            var sourceItem = ItemHolder.Instance.GetItemInfoByKey(sourceItemName);
            //TODO: make more optimization
            if (sourceItem.IsCraftable)
            {
                var playerInventory = GlobalPlayer.Instance.PlayerInventory;
                Debug.Assert(playerInventory != null, "Player inventory is NULL!");
                int craftableCount = 0;
                foreach (var craftPartItem in sourceItem.ItemCraftParts)
                {
                    var itemInInventory = playerInventory.GetItemByName(craftPartItem.ItemPartName);
                    if (itemInInventory != null && itemInInventory.Amount >= craftPartItem.Amount)
                    {
                        craftableCount++;
                    }
                }

                var isCanCraft = craftableCount >= sourceItem.ItemCraftParts.Count;
                if (isCanCraft)
                {
                    foreach (var craftPartItem in sourceItem.ItemCraftParts)
                    {
                        var itemInInventory = playerInventory.GetItemByName(craftPartItem.ItemPartName);
                        if (itemInInventory != null)
                        {
                            for (int i = 0; i < craftPartItem.Amount; i++)
                            {
                                playerInventory.RemoveItem(craftPartItem.ItemPartName);
                            }
                        }
                    }

                    return new InventoryItem()
                    {
                        Amount = 1,
                        ItemName = sourceItemName
                    };
                }
            }
            Debug.LogWarning("Cannot craft item!");
            return null;
        }
        
    }
}
