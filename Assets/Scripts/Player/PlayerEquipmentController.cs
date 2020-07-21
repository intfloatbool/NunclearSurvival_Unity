﻿using System;
using Player;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerEquipmentController
    {
        private GlobalPlayer _globalPlayer;
        private PlayerInventory _playerInventory;
        private PlayerEquipment _currentEquipment;
        
        public event Action<PlayerEquipment> OnEquipmentChanged;
        
        public PlayerEquipmentController(GlobalPlayer globalPlayer)
        {
            this._globalPlayer = globalPlayer;
            this._playerInventory = globalPlayer.PlayerInventory;
            Assert.IsNotNull(_playerInventory, "_playerInventory != null");
            if (_playerInventory != null)
            {
                this._playerInventory.OnItemsUpdated += CheckEquipmentContainsInInventory;   
            }
        }

        public void Init()
        {
            //TODO: load values 
            _currentEquipment = _globalPlayer.PlayerInfoProvider.LoadEquipment();
            CheckEquipmentContainsInInventory();
            OnEquipmentChanged?.Invoke(_currentEquipment);
        }

        private void CheckEquipmentContainsInInventory()
        {
            var weaponName = _currentEquipment.Weapon.ItemName;
            if (weaponName != ItemName.NONE)
            {
                var itemRef = _playerInventory.GetItemByName(weaponName);
                if (itemRef == null)
                {
                    UnEquip(ItemType.EQUIPMENT_WEAPON);
                }
            }
            
            var armorName = _currentEquipment.Armor.ItemName;
            if (armorName != ItemName.NONE)
            {
                var itemRef = _playerInventory.GetItemByName(armorName);
                if (itemRef == null)
                {
                    UnEquip(ItemType.EQUIPMENT_ARMOR);
                }
            }
            
            var grenadeName = _currentEquipment.Grenade.ItemName;
            if (grenadeName != ItemName.NONE)
            {
                var itemRef = _playerInventory.GetItemByName(grenadeName);
                if (itemRef == null)
                {
                    UnEquip(ItemType.EQUIPMENT_GRENADE);
                }
            }
            
            var meleeWeaponName = _currentEquipment.MeleeWeapon.ItemName;
            if (meleeWeaponName != ItemName.NONE)
            {
                var itemRef = _playerInventory.GetItemByName(meleeWeaponName);
                if (itemRef == null)
                {
                    UnEquip(ItemType.EQUIPMENT_MELEE_WEAPON);
                }
            }
        }

        public bool IsEquipable(ItemInfo itemInfo)
        {
            return itemInfo.ItemType == ItemType.EQUIPMENT_ARMOR ||
                   itemInfo.ItemType == ItemType.EQUIPMENT_WEAPON ||
                   itemInfo.ItemType == ItemType.EQUIPMENT_GRENADE ||
                   itemInfo.ItemType == ItemType.EQUIPMENT_MELEE_WEAPON;
        }
        
        public void UnEquip(ItemType itemType)
        {
            PlayerEquipment lastValues = _currentEquipment;
            PlayerEquipment freshValues = lastValues;
            switch (itemType)
            {
                case ItemType.EQUIPMENT_ARMOR:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        new EquipmentValue(itemType, ItemName.NONE),
                        lastValues.MeleeWeapon,
                        lastValues.Grenade
                    );
                    break;
                }
                case ItemType.EQUIPMENT_WEAPON:
                {
                    freshValues = new PlayerEquipment(
                        new EquipmentValue(itemType, ItemName.NONE),
                        lastValues.Armor,
                        lastValues.MeleeWeapon,
                        lastValues.Grenade
                    );
                    break;
                }
                case ItemType.EQUIPMENT_GRENADE:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        lastValues.Armor,
                        lastValues.MeleeWeapon,
                        new EquipmentValue(itemType, ItemName.NONE)
                    );
                    break;
                }
                case ItemType.EQUIPMENT_MELEE_WEAPON:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        lastValues.Armor,
                        new EquipmentValue(itemType, ItemName.NONE),
                        lastValues.Grenade
                    );
                    break;
                }
                default:
                {
                    Debug.LogError($"ItemType- {itemType} is not equipable!!!");
                    return;
                }
            }

            _currentEquipment = freshValues;
            OnEquipmentChanged?.Invoke(freshValues);
        }
        
        public void Equip(ItemInfo itemInfo)
        {
            var itemType = itemInfo.ItemType;
            PlayerEquipment lastValues = _currentEquipment;
            PlayerEquipment freshValues = lastValues;
            switch (itemType)
            {
                case ItemType.EQUIPMENT_ARMOR:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        new EquipmentValue(itemType, itemInfo.ItemName),
                        lastValues.MeleeWeapon,
                        lastValues.Grenade
                        );
                    break;
                }
                case ItemType.EQUIPMENT_WEAPON:
                {
                    freshValues = new PlayerEquipment(
                        new EquipmentValue(itemType, itemInfo.ItemName),
                        lastValues.Armor,
                        lastValues.MeleeWeapon,
                        lastValues.Grenade
                    );
                    break;
                }
                case ItemType.EQUIPMENT_GRENADE:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        lastValues.Armor,
                        lastValues.MeleeWeapon,
                        new EquipmentValue(itemType, itemInfo.ItemName)
                    );
                    break;
                }
                case ItemType.EQUIPMENT_MELEE_WEAPON:
                {
                    freshValues = new PlayerEquipment(
                        lastValues.Weapon,
                        lastValues.Armor,
                        new EquipmentValue(itemType, itemInfo.ItemName),
                        lastValues.Grenade
                    );
                    break;
                }
                default:
                {
                    Debug.LogError($"Item {itemInfo.ItemName} is not equipable!!!");
                    return;
                }
            }
            _currentEquipment = freshValues;
            OnEquipmentChanged?.Invoke(freshValues);
            
        }

    }
}
