using System.Collections.Generic;
using Common.Dependencies;
using Common.Interfaces.Collections;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class ItemHolder : UnitySingletonBase<ItemHolder>, ISingletonDependency
    {
        [SerializeField] private List<ItemInfo> _itemInfos;
        public List<ItemInfo> ItemInfos => _itemInfos;
        private Dictionary<ItemName, ItemInfo> _itemInfoDict = new Dictionary<ItemName, ItemInfo>();
        protected override ItemHolder GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();
            InitDict();
        }

        private void InitDict()
        {
            foreach (var itemInfo in _itemInfos)
            {
                var key = itemInfo.ItemName;
                if (_itemInfoDict.ContainsKey(key))
                {
                    Debug.LogError($"Cannot initialize itemInfo with key {key}! Already defined!");
                    continue;
                }
                else
                {
                    _itemInfoDict.Add(key, itemInfo);
                }
            }
        }

        public ItemInfo GetItemInfoByKey(ItemName itemName)
        {
            return _itemInfoDict.ContainsKey(itemName) ? _itemInfoDict[itemName] : null;
        }

        public void SelfRegister()
        {
            DepResolver.RegisterDependency(this);
        }
    }
}
