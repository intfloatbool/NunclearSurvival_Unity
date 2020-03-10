using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private InventoryItemUi _itemUiPrefab;
    private List<InventoryItemUi> _currentItems = new List<InventoryItemUi>();

    private void Start()
    {
        UpdateItems();
    }

    public void UpdateItems()
    {
        _currentItems.ForEach(i => Destroy(i.gameObject));
        _currentItems.Clear();
        var playerItems = GlobalPlayer.Instance.InventoryItems;
        foreach(var playerItem in playerItems)
        {
            var itemInfo = ItemHolder.Instance.GetItemInfoByKey(playerItem.ItemName);
            if(itemInfo != null)
            {
                var itemUi = Instantiate(_itemUiPrefab, _itemsParent);
                itemUi.gameObject.SetActive(true);
                itemUi.UpdateItem(this, itemInfo, playerItem.Amount);
                _currentItems.Add(itemUi);
            }
            else
            {
                Debug.LogError($"ItemInfo with name {playerItem.ItemName} not found!");
            }
        }
    }
}
