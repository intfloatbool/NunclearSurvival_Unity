using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class ItemVisualHolder : UnitySingletonBase<ItemVisualHolder>
    {
        [SerializeField] private ItemVisualData[] _itemVisualData;
        private Dictionary<ItemName, ItemVisualData> _itemVisualDataDict;
        
        /// <summary>
        /// Can return null
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public ItemVisualData GetVisualDataByKey(ItemName itemName)
        {
            ItemVisualData data = null;
            _itemVisualDataDict.TryGetValue(itemName, out data);

            return data;
        }

        protected override ItemVisualHolder GetInstance()
        {
            return this;
        }

        protected override void Awake()
        {
            base.Awake();

            _itemVisualDataDict = _itemVisualData.ToDictionary(item => item.GetKey());
        }
    }
}