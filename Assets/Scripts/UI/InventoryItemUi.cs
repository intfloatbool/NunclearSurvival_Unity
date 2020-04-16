﻿using GameUI;
using SingletonsPreloaders;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUi : GameButtonBase
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private TextMeshProUGUI _amountText;
    private ItemInfo _currentItemInfo;
    public ItemInfo CurrentItemInfo => _currentItemInfo;
    private IItemHandler _itemHandler;

    private int _currentAmount;
    public int CurrentAmount => _currentAmount;
    public bool IsDragged { get; private set; }

    public Action<InventoryItemUi> ExternalOnClickAction { get; set; }

    public void UpdateItem(IItemHandler itemHandler, ItemInfo itemInfo, int amount)
    {
        _currentItemInfo = itemInfo;
        _iconImg.sprite = _currentItemInfo.ItemIcon;
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
        _itemHandler.MakeActionWithItem(this);
    }

    public void OnItemDrag()
    {
        IsDragged = true;
        _itemHandler.OnItemDrag(this);
    }

    public void OnItemUp()
    {
        IsDragged = false;
        _itemHandler.OnItemUp(this);       
    }

    public InventoryItemUi Clone(Transform parent = null)
    {
        var clone = Instantiate(this, parent);
        clone.UpdateItem(_itemHandler, _currentItemInfo, _currentAmount);
        return clone;
    }
}
