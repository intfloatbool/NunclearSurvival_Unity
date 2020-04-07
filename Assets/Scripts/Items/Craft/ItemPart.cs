using System;
using UnityEngine;

[Serializable]
public class ItemPart
{
    [SerializeField] private ItemName _itemPartName;
    public ItemName ItemPartName => _itemPartName;

    [SerializeField] private int _amount = 1;
    public int Amount => _amount;
}
