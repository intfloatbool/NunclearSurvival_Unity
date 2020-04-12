using System;
using System.Collections.Generic;
using GameUI;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private InventoryItemUi _itemUiPrefab;
    private List<InventoryItemUi> _currentItems = new List<InventoryItemUi>();

    [SerializeField] private string _useItemLocKey = "action_item_use";
    [SerializeField] private string _dropItemLocKey = "action_item_drop";

    [Space(3f)]
    [SerializeField] private Image _draggedItemImage;

    private ItemInfo _lastClickedItem;

    private Dictionary<ItemType, List<InventoryItemUi>> _categorizedItemsUiDict = new Dictionary<ItemType, List<InventoryItemUi>>();
    private ItemType[] _lastCategories;

    [Space(5f)]
    [Header("Runtime references")]
    [SerializeField] private InventoryItemUi _draggedItem;
    

    private void Awake()
    {
        TryRemoveDebugItemsBeforLoad();
    }

    public void ShowItemsByCategory(params ItemType[] itemTypes)
    {      
        UpdateItems();
        for(int i = 0; i < itemTypes.Length; i++)
        {
            var itemType = itemTypes[i];
            var itemsByCategory = _categorizedItemsUiDict.ContainsKey(itemType) ? _categorizedItemsUiDict[itemType] : null;
            if (itemsByCategory == null)
            {
                _categorizedItemsUiDict.Add(itemType, new List<InventoryItemUi>());
                itemsByCategory = _categorizedItemsUiDict[itemType];
            }

            _currentItems.AddRange(itemsByCategory);
        }

        _lastCategories = itemTypes;
        ShowCurrentItems();
    }

    
    private void ShowCurrentItems()
    {
        _currentItems.ForEach(i => i.gameObject.SetActive(true));
    }

    private void DestroyCurrentItems()
    {
        _currentItems.ForEach(i => Destroy(i.gameObject));
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
        DestroyCurrentItems();
        _currentItems.Clear();
        foreach(var itemType in _categorizedItemsUiDict.Keys)
        {
            var items = _categorizedItemsUiDict[itemType];
            items.ForEach(i => Destroy(i.gameObject));
            items.Clear();
        }
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
            var itemCategory = itemInfo.ItemType;
            var itemsByCategory = _categorizedItemsUiDict.ContainsKey(itemCategory) ? _categorizedItemsUiDict[itemCategory] : null;
            if(itemsByCategory == null)
            {
                _categorizedItemsUiDict.Add(itemCategory, new List<InventoryItemUi>());
                itemsByCategory = _categorizedItemsUiDict[itemCategory];
            }
            var itemUi = Instantiate(_itemUiPrefab, _itemsParent);
            itemUi.gameObject.SetActive(false);
            itemUi.UpdateItem(this, itemInfo, item.Amount);
            itemsByCategory.Add(itemUi);       
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

        var globalPlayer = GlobalPlayer.Instance;
        if(globalPlayer == null)
        {
            Debug.LogError("Global player singleton is missing!");
            return;
        }

        var inventory = globalPlayer.PlayerInventory;
        inventory.RemoveItem(_lastClickedItem.ItemName);

        if(_lastCategories != null)
        {
            ShowItemsByCategory(_lastCategories);
        }
    }

    public void OnItemDrag(InventoryItemUi itemUi)
    {
        if(_draggedItem != itemUi)
        {
            _draggedItemImage.sprite = itemUi.CurrentItemInfo.ItemIcon;
            _draggedItemImage.gameObject.SetActive(true);
        }
        _draggedItem = itemUi;      
    }

    public void OnItemUp(InventoryItemUi itemUi)
    {
        _draggedItem = null;
        _draggedItemImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_draggedItem != null && _draggedItemImage != null)
        {
            _draggedItemImage.rectTransform.position = Input.mousePosition;
        }
    }
}
