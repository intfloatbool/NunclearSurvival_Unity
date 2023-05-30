using SingletonsPreloaders;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    /// <summary>
    /// Only for debug.
    /// </summary>
    /// 
    public string Name;
    [SerializeField] private int _amount;
    public int Amount
    {
        get { return _amount; }
        set { this._amount = value; }
    }
    [SerializeField] private ItemName _itemName;
    public ItemName ItemName
    {
        get { return this._itemName; }
        set
        {
            this.Name = value.ToString();
            this._itemName = value;
        }
    }

    private ItemInfo _cachedInfo;
    public ItemInfo ItemInfo 
    {
        get
        {
            if(_cachedInfo == null)
            {
                _cachedInfo = ItemHolder.Instance.GetItemInfoByKey(_itemName);
                if(_cachedInfo == null)
                {
                    Debug.LogError("Cannot get item info from item holder! Not exists!");
                }
            }

            return _cachedInfo;
        }
    }

    public InventoryItem Clone()
    {
        return MemberwiseClone() as InventoryItem;
    }
    
}
