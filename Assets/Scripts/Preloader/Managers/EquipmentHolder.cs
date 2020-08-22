using System.Collections.Generic;
using Common.Dependencies;
using NunclearGame.Items;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace SingletonsPreloaders
{
    public class EquipmentHolder : UnitySingletonBase<EquipmentHolder>, ISingletonDependency
    {
        [SerializeField] private EquipmentItemInfo[] _equipmentItemInfoCollection;
        private Dictionary<ItemName, EquipmentItemInfo> _itemsDict;
        protected override EquipmentHolder GetInstance() => this;

        protected override void Awake()
        {
            base.Awake();

            _itemsDict =
                GameHelper.InitDictionaryByCollection<ItemName, EquipmentItemInfo>(_equipmentItemInfoCollection);
            
            Assert.IsTrue(_equipmentItemInfoCollection.Length > 0, "_equipmentItemInfoCollection.Length > 0");
            Assert.IsNotNull(_itemsDict, "_itemsDict != null");
            Assert.IsTrue(_itemsDict.Values.Count > 0, "_itemsDict.Values.Count > 0");
        }

        public EquipmentItemInfo GetPlayerEquipmentByType(ItemType itemType)
        {
            if (GameHelper.GlobalPlayer == null || GameHelper.GlobalPlayer.EquipmentController == null)
            {
                Debug.LogError("Global player or Equipment is MISSING!");
                return null;
            }
            
            var playerEquipment = GameHelper.GlobalPlayer.EquipmentController.CurrentEquipment;
            var allEquipment = playerEquipment.GetAllEquipment();
            for (int i = 0; i < allEquipment.Length; i++)
            {
                var equipment = allEquipment[i];
                if (equipment.ItemType == itemType)
                {
                    if (equipment.ItemName == ItemName.NONE)
                    {
                        return null;
                    }
                    var item = GetItemByName(equipment.ItemName);
                    if (item == null)
                    {
                        Debug.LogError($"No equipmentInfo for item {equipment.ItemName}!");
                        return null;
                    }

                    return item;
                }
            }
            Debug.LogError($"There is no equipment with type {itemType}!");
            return null;
        }

        public EquipmentItemInfo GetItemByName(ItemName itemName)
        {
            EquipmentItemInfo itemInfo = null;

            _itemsDict.TryGetValue(itemName, out itemInfo);

            return itemInfo;
        }

        public void SelfRegister()
        {
            DepResolver.RegisterDependency(this);
        }
    }
}

