using System.Collections.Generic;
using GameUI;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;
using Player;
using UnityEngine.Assertions;

public class UiInventory : MonoBehaviour, IItemHandler
{
    
    [SerializeField] private Transform _itemsParent;
    public Transform ItemsParent => _itemsParent;

    [SerializeField] private InventoryItemUi _itemUiPrefab;
    private List<InventoryItemUi> _currentItems = new List<InventoryItemUi>();

    private List<InventoryItemUi> _allItems = new List<InventoryItemUi>();
    public List<InventoryItemUi> AllItems => _allItems;

    [SerializeField] private string _useItemLocKey = "action_item_use";
    [SerializeField] private string _dropItemLocKey = "action_item_drop";

    [Space(3f)]
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    [Space(3f)]
    [SerializeField] private Image _draggedItemImage;

    private ItemInfo _lastClickedItem;

    private Dictionary<ItemType, List<InventoryItemUi>> _categorizedItemsUiDict = new Dictionary<ItemType, List<InventoryItemUi>>();
    private ItemType[] _lastCategories;

    [Space(5f)]
    [Header("Runtime references")]
    [SerializeField] private InventoryItemUi _draggedItem;
    private PointerEventData m_PointerEventData;

    private PlayerInventory _playerInventory;
    
    public event Action OnItemsUpdated;
    
    private void Awake()
    {
        _playerInventory = GlobalPlayer.Instance?.PlayerInventory;
        Assert.IsNotNull(_playerInventory, "_playerInventory != null");
        if (_playerInventory != null)
        {
            _playerInventory.OnItemsUpdated += UpdateItems;
            _playerInventory.OnItemsUpdated += UpdateLastCategories;
        }
        TryRemoveDebugItemsBeforLoad();
    }

    private void OnDestroy()
    {
        if (_playerInventory != null)
        {
            _playerInventory.OnItemsUpdated -= UpdateItems;
            _playerInventory.OnItemsUpdated -= UpdateLastCategories;
        }
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

    public InventoryItemUi GetItemByName(ItemName itemName)
    {
        return _allItems.FirstOrDefault(iui => iui.CurrentItemInfo.ItemName == itemName);
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
        _allItems.Clear();
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

        OnItemsUpdated?.Invoke();
    }

    public void AddItem(InventoryItem item)
    {
        var itemInfo = ItemHolder.Instance.GetItemInfoByKey(item.ItemName);
        AddItem(itemInfo, item.Amount);
    }

    public void AddItem(ItemInfo itemInfo, int amount)
    {
        if (itemInfo != null)
        {
            var itemCategory = itemInfo.ItemType;
            var itemsByCategory = _categorizedItemsUiDict.ContainsKey(itemCategory) ? _categorizedItemsUiDict[itemCategory] : null;
            if (itemsByCategory == null)
            {
                _categorizedItemsUiDict.Add(itemCategory, new List<InventoryItemUi>());
                itemsByCategory = _categorizedItemsUiDict[itemCategory];
            }
            var itemUi = Instantiate(_itemUiPrefab, _itemsParent);
            itemUi.gameObject.SetActive(false);
            itemUi.UpdateItem(this, itemInfo, amount);
            itemsByCategory.Add(itemUi);
            _allItems.Add(itemUi);
        }
        else
        {
            Debug.LogError($"ItemInfo with name {itemInfo.ItemName} not found!");
        }
    }

    public void OnItemDropAtHandler(InventoryItemUi itemUi)
    {
        AddItem(itemUi.CurrentItemInfo, itemUi.CurrentAmount);
    }

    public void MakeActionWithItem(InventoryItemUi itemUi)
    {
        ShowDialogAboutItem(itemUi.CurrentItemInfo);
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
        RemoveItem(_lastClickedItem);
    }

    public void RemoveItem(ItemInfo itemInfo)
    {
        var globalPlayer = GlobalPlayer.Instance;
        if (globalPlayer == null)
        {
            Debug.LogError("Global player singleton is missing!");
            return;
        }

        var inventory = globalPlayer.PlayerInventory;
        inventory.RemoveItem(itemInfo.ItemName);
        UpdateLastCategories();       
    }

    public void UpdateLastCategories() 
    {
        if (_lastCategories != null)
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

        TryUseItemUiInInteractors(itemUi);
    }

    private void TryUseItemUiInInteractors(InventoryItemUi itemUi)
    {
        m_PointerEventData = new PointerEventData(_eventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(m_PointerEventData, results);

        IItemInteractorUI interactor = null;

        foreach (RaycastResult result in results)
        {
            interactor = result.gameObject.GetComponent<IItemInteractorUI>();
            if (interactor != null)
                break;
        }

        if(interactor != null)
        {
            interactor.OnItemDroppedHere(itemUi);
        }
    }

    private void Update()
    {
        if(_draggedItem != null && _draggedItemImage != null)
        {
            _draggedItemImage.rectTransform.position = Input.mousePosition;
        }
    }
}
