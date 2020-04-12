using System.Collections.Generic;
using GameUI;
using SingletonsPreloaders;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private InventoryItemUi _itemUiPrefab;
    private List<InventoryItemUi> _currentItems = new List<InventoryItemUi>();

    [SerializeField] private string _useItemLocKey = "action_item_use";
    [SerializeField] private string _dropItemLocKey = "action_item_drop";

    private ItemInfo _lastClickedItem;

    private void Start()
    {
        TryRemoveDebugItemsBeforLoad();
        UpdateItems();
    }

    private void TryRemoveDebugItemsBeforLoad()
    {
        var debugItems = transform.GetComponentsInChildren<InventoryItemUi>();
        if(debugItems.Length > 0)
        {
            for(int i = 0; i < debugItems.Length; i++)
            {
                var debugItem = debugItems[i];
                Destroy(debugItem.gameObject);
            }
        }
    }

    public void UpdateItems()
    {
        _currentItems.ForEach(i => Destroy(i.gameObject));
        _currentItems.Clear();
        var playerItems = GlobalPlayer.Instance.PlayerInventory.GetCurrentItems();
        foreach(var playerItem in playerItems)
        {
            AddItem(playerItem);
        }
    }

    public void AddItem(InventoryItem item)
    {
        var itemInfo = ItemHolder.Instance.GetItemInfoByKey(item.ItemName);
        if (itemInfo != null)
        {
            var itemUi = Instantiate(_itemUiPrefab, _itemsParent);
            itemUi.gameObject.SetActive(true);
            itemUi.UpdateItem(this, itemInfo, item.Amount);
            _currentItems.Add(itemUi);
        }
        else
        {
            Debug.LogError($"ItemInfo with name {item.ItemName} not found!");
        }
    }

    public void ShowDialogAboutItem(ItemInfo itemInfo)
    {
        var commongGui = CommonGui.Instance;
        if(commongGui == null)
        {
            Debug.LogError("CommongGui singleton is missing!");
            return;
        }

        var dialog = commongGui.GetDialog();
        if(dialog == null)
        {
            Debug.LogError("Dialog is missing!");
            return;
        }

        _lastClickedItem = itemInfo;

        dialog
            .ResetDialog()
            .SetHeader(itemInfo.ItemViewNameKey, itemInfo.ItemIcon)
            .SetDialogDescription(itemInfo.DescriptionLocKey)
            .AddButton(_dropItemLocKey, OnItemDrop, CustomDialog.DialogPartType.DANGER)
            .AddButton(_useItemLocKey, OnItemUse, CustomDialog.DialogPartType.ACCESS)           
            .ShowDialog();
    }

    private void OnItemUse()
    {
        //TODO: Complete futher logic!
        Debug.Log("Item use! - " + _lastClickedItem.ItemViewNameKey);
    }

    private void OnItemDrop()
    {
        //TODO: Complete futher logic!
        Debug.Log("Item dropped! - " + _lastClickedItem.ItemViewNameKey);
    }


}
