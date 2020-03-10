using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalPlayer : UnitySingletonBase<GlobalPlayer>
{
    [SerializeField] private List<InventoryItem> _currentItems;
    public List<InventoryItem> InventoryItems => _currentItems;
    protected override GlobalPlayer GetInstance() => this;

    protected override void Awake()
    {
        base.Awake();
        LoadExistingItems();
    }

    private void LoadExistingItems()
    {
        //TODO: Realize loading items from internal store...
        //Fake store
        AddItem(ItemName.FOOD_APPLE);
        AddItem(ItemName.FOOD_APPLE);
        AddItem(ItemName.FOOD_CHICKEN);
        AddItem(ItemName.WATER_WATER);
        AddItem(ItemName.WATER_WATER);
        AddItem(ItemName.WATER_WATER);
    }

    public void AddItem(ItemName itemName)
    {
        var sameItem = _currentItems.FirstOrDefault(c => c.ItemName == itemName);
        if(sameItem != null)
        {
            sameItem.Amount++;
        }
        else
        {
            _currentItems.Add(new InventoryItem()
            {
                ItemName = itemName,
                Amount = 1
            });
        }
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
               
        }
        else
        {
            Debug.LogError($"Cannot delete item from player inventory with key {itemName} ! Not exist!");
        }
    }
}
