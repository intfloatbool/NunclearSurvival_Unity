using System;
using System.Collections.Generic;
using System.Linq;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private EquipViewItem[] _equipViewItems;
        private Dictionary<ItemType, EquipViewItem> _viewItemsDict = new Dictionary<ItemType, EquipViewItem>();

        public event Action<ItemType> OnItemViewEquipped;
        public event Action<ItemType> OnItemUnequipped;
        private void Awake()
        {
            if (_equipViewItems == null || _equipViewItems.Length <= 0)
            {
                _equipViewItems = GetComponentsInChildren<EquipViewItem>();
            }
            
            Assert.IsTrue(_equipViewItems.Length > 0);

            _viewItemsDict = GameHelper.InitDictionaryByCollection<ItemType, EquipViewItem>(_equipViewItems);
            Assert.IsTrue(_viewItemsDict.Values.Count > 0, "_viewItemsDict.Values.Count > 0");
        }

        public EquipViewItem[] GetViewItems()
        {
            return _equipViewItems.ToArray();
        }

        public void EquipItemByType(ItemType itemType, GameObject itemPrefab)
        {
            if (!_viewItemsDict.ContainsKey(itemType))
            {
                Debug.LogError($"There is no items anchors  for  type {itemType}!");
                return;
            }
            
            _viewItemsDict[itemType].SetItem(itemPrefab);
            OnItemViewEquipped?.Invoke(itemType);
        }

        public void UnequipItemByType(ItemType itemType)
        {
            if (!_viewItemsDict.ContainsKey(itemType))
            {
                Debug.LogError($"There is no items anchors  for  type {itemType}!");
                return;
            }
            
            _viewItemsDict[itemType].SetItem(null);
            
            OnItemUnequipped?.Invoke(itemType);
        }
    }
}
