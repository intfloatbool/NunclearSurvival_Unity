using System;
using System.Collections.Generic;
using NunclearGame.Items;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    [Header("This is localization key of name")]
    [SerializeField] private string _itemViewNameKey;
    public string ItemViewNameKey => _itemViewNameKey;

    [Space(3f)]
    [SerializeField] private string _descriptionLocKey;
    public string DescriptionLocKey => _descriptionLocKey;

    [SerializeField] private ItemName _itemName;
    /// <summary>
    /// Primary key of item
    /// </summary>
    public ItemName ItemName => _itemName;
    [SerializeField] private ItemType _itemType;
    public ItemType ItemType => _itemType;
    [SerializeField] private Sprite _itemIcon;
    public Sprite ItemIcon => _itemIcon;

    [Space(3f)]
    [SerializeField] private bool _isConstantItem;
    public bool IsConstantItem => _isConstantItem;

    [Space(5f)] 
    [Header("Item using zone")] 
    [SerializeField] private ItemValue[] _itemValues;
    
    [Space(5f)] 
    [Header("Craft zone:")]
    [SerializeField] private bool _isCraftable;
    public bool IsCraftable => _isCraftable;
    [SerializeField] private List<ItemPart> _itemCraftParts;
    public List<ItemPart> ItemCraftParts => _itemCraftParts;

    
    private Dictionary<string, int> _itemValuesDict;

    public IEnumerable<ItemValue> GetItemValues()
    {
        return _itemValues;
    }
    
    public int? GetItemValueByKey(string key)
    {
        if (_itemValuesDict == null)
        {
            InitValuesDict();
        }

        if (_itemValuesDict.ContainsKey(key))
            return _itemValuesDict[key];
        
        return null;
    }

    private void InitValuesDict()
    {
        _itemValuesDict = new Dictionary<string, int>();
        for (int i = 0; i < _itemValues.Length; i++)
        {
            var itemValue = _itemValues[i];
            if (itemValue != null)
            {
                if (!_itemValuesDict.ContainsKey(itemValue.ValueKey))
                {
                    _itemValuesDict.Add(itemValue.ValueKey, itemValue.Value);
                }
                else
                {
                    Debug.LogError($"Item already have value with key: " + itemValue.ValueKey);
                }
            }
        }
    }
    
    public ItemInfo Clone() 
    {
        var boxingClone = MemberwiseClone();
        return boxingClone as ItemInfo;
    }

}
