using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    [SerializeField] private string _itemViewNameKey;
    public string ItemViewNameKey => _itemViewNameKey;
    [SerializeField] private ItemName _itemName;
    /// <summary>
    /// Primary key of item
    /// </summary>
    public ItemName ItemName => _itemName;
    [SerializeField] private ItemType _itemType;
    public ItemType ItemType => _itemType;
    [SerializeField] private Sprite _itemIcon;
    public Sprite ItemIcon => _itemIcon;
}
