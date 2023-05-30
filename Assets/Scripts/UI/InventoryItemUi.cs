using GameUI;
using SingletonsPreloaders;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUi : GameButtonBase
{
    [SerializeField] private Image _iconImg;
    private Sprite _itemSprite;
    public Sprite ItemSprite 
    {
        get { return _itemSprite; }
        set 
        {
            this._itemSprite = value;
            _iconImg.sprite = _itemSprite;
        }
    }
    [SerializeField] private TextMeshProUGUI _amountText;
    private ItemInfo _currentItemInfo;
    public ItemInfo CurrentItemInfo => _currentItemInfo;
    private IItemHandler _itemHandler;

    private int _currentAmount;
    public int CurrentAmount
    {
        get { return _currentAmount; }
        set 
        {
            this._currentAmount = value;
            this._amountText.text =  _currentAmount.ToString();
        }
    }
    public bool IsDragged { get; private set; }

    public Action<InventoryItemUi> ExternalOnClickAction { get; set; }

    public void UpdateItem(IItemHandler itemHandler, ItemInfo itemInfo, int amount)
    {
        _currentItemInfo = itemInfo;
        _iconImg.sprite = _currentItemInfo.ItemIcon;
        _itemSprite = _currentItemInfo.ItemIcon;
        if(!itemInfo.IsConstantItem)
        {
            _amountText.gameObject.SetActive(true);
            _currentAmount = amount; 
            _amountText.text = _currentAmount.ToString();
        }
        _itemHandler = itemHandler;
    }

    protected override void OnClick()
    {
        if(ExternalOnClickAction != null)
        {
            ExternalOnClickAction(this);
            return;
        }
        _itemHandler?.MakeActionWithItem(this);
    }

    public void OnItemDrag()
    {
        IsDragged = true;
        if (_itemHandler != null)
        {
            _itemHandler.OnItemDrag(this);
        }
        
    }

    public void OnItemUp()
    {
        IsDragged = false;
        if (_itemHandler != null)
        {
            _itemHandler.OnItemUp(this);   
        }
    }

    public InventoryItemUi Clone(Transform parent = null)
    {
        var clone = Instantiate(this, parent);
        clone.UpdateItem(_itemHandler, _currentItemInfo, _currentAmount);
        return clone;
    }
}
