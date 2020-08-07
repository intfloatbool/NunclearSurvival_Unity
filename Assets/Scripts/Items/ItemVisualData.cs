using System;
using Common.Interfaces.Collections;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemVisualData : IPrimaryKeyable<ItemName>
    {
        [SerializeField] private ItemName _itemName;
        [SerializeField] private GameObject _prefab;
        
        public GameObject Prefab => _prefab;
        
        public ItemName GetKey()
        {
            return _itemName;
        }
    }
}