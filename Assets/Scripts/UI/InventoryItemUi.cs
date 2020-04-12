using SingletonsPreloaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUi : GameButtonBase
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private TextMeshProUGUI _amountText;
    private ItemInfo _currentItemInfo;
    public ItemInfo CurrentItemInfo => _currentItemInfo;
    private UiInventory _inventory;

    public bool IsDragged { get; private set; }

    public void UpdateItem(UiInventory inventory, ItemInfo itemInfo, int amount)
    {
        _currentItemInfo = itemInfo;
        _iconImg.sprite = _currentItemInfo.ItemIcon;
        if(!itemInfo.IsConstantItem)
        {
            _amountText.gameObject.SetActive(true);
            _amountText.text = amount.ToString();
        }   
        _inventory = inventory;
    }

    protected override void OnClick()
    {
        _inventory.ShowDialogAboutItem(_currentItemInfo);
        //var globalPlayer = GlobalPlayer.Instance;
        //globalPlayer.PlayerInventory.RemoveItem(_currentItemInfo.ItemName);
        //_inventory.UpdateItems();
    }

    public void OnItemDrag()
    {
        IsDragged = true;
        _inventory.OnItemDrag(this);  
    }

    public void OnItemUp()
    {
        IsDragged = false;
        _inventory.OnItemUp(this);       
    }
}
