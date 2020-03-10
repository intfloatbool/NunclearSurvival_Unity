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
}
