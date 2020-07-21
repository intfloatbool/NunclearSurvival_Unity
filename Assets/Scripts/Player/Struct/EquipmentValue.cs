using UnityEngine;

namespace NunclearGame.Player
{
    [System.Serializable]
    public struct EquipmentValue
    {
        [SerializeField] private ItemType _itemType;
        public ItemType ItemType => _itemType;

        [SerializeField] private ItemName _itemName;
        public ItemName ItemName => _itemName;

        public EquipmentValue(ItemType itemType, ItemName itemName)
        {
            this._itemType = itemType;
            this._itemName = itemName;
        }
    }
}

