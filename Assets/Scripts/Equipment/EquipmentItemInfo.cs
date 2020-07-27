using Common.Interfaces;
using UnityEngine;

namespace NunclearGame.Items
{
    [System.Serializable]
    public class EquipmentItemInfo: IKeyPreferer<ItemName>
    {
        [SerializeField] private ItemName _itemName;
        public ItemName ItemName => _itemName;

        [SerializeField] private ItemType _itemType;
        public ItemType ItemType => _itemType;

        [SerializeField] private GameObject _relativePrefab;
        public GameObject RelativePrefab => _relativePrefab;
        public ItemName GetKey()
        {
            return _itemName;
        }
    }
}
