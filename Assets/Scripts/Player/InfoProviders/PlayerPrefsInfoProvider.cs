using System;
using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;

namespace Player
{
    public class PlayerPrefsInfoProvider : PlayerInfoProviderBase
    {
        private readonly string PlayerNameKey = "playerNickName";
        public override string LoadPlayerName()
        {
            return PlayerPrefs.GetString(PlayerNameKey);
        }

        public override void SetPlayerName(string freshName)
        {
            PlayerPrefs.SetString(PlayerNameKey, freshName);
        }

        public override PlayerInventory LoadInventory()
        {
            var playerInventory = new PlayerInventory();
            var itemNames = Enum.GetNames(typeof(ItemName));
            for (int i = 0; i < itemNames.Length; i++)
            {
                var itemNameStr = itemNames[i];
                var itemCount = PlayerPrefs.GetInt(itemNameStr, -1);
                if (itemCount > 0)
                {
                    for (int j = 0; j < itemCount; j++)
                    {
                        var itemName = (ItemName) i;
                        playerInventory.AddItem(itemName);
                    }
                }
            }
            //subscribe to events, to change item values dynamically
            playerInventory.OnItemAdded += OnItemAdded;
            playerInventory.OnItemRemoved += OnItemRemoved;
            
            return playerInventory;
        }

        public override PlayerValues LoadPlayerValues()
        {
            bool isPlayerValuesExists = PlayerPrefs.HasKey(GameHelper.PlayerPrefsKeys.HAS_VALUES_KEY);
            
            if (isPlayerValuesExists == false)
            {
                PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.HAS_VALUES_KEY, 1);
                var defaultValues = GameHelper.PlayerHelper.CreateDefaultValues();
                SavePlayerValues(defaultValues);
            }

            return GetPlayerValues();
        }

        public override void SavePlayerValues(PlayerValues playerValues)
        {
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.MAX_HP_KEY, playerValues.MaxHp);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.RATING_KEY, playerValues.Rating);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.CURRENT_HP_KEY, playerValues.CurrentHp);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.MAX_STAMINA_KEY, playerValues.MaxStamina);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.PLAYER_LEVEL_KEY, playerValues.PlayerLvl);
        }

        private PlayerValues GetPlayerValues()
        {
            int maxHp = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.MAX_HP_KEY, 0);
            int rating = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.RATING_KEY, 0);
            int currentHp = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.CURRENT_HP_KEY, 0);
            int maxStamina = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.MAX_STAMINA_KEY, 0);
            int playerLvl = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.PLAYER_LEVEL_KEY, 0);
            
            return new PlayerValues(
                playerLvl, maxHp, currentHp, maxStamina, rating
                );
        }

        private void OnItemAdded(ItemName itemName, int currentAmount)
        {
            var itemNameStr = itemName.ToString();
            var currentItemValue = PlayerPrefs.GetInt(itemNameStr, -1);
            PlayerPrefs.SetInt(itemNameStr, currentItemValue + 1);
        }
        
        private void OnItemRemoved(ItemName itemName)
        {
            var itemNameStr = itemName.ToString();
            var currentItemValue = PlayerPrefs.GetInt(itemNameStr, -1);
            if (currentItemValue >= 0)
            {
                PlayerPrefs.SetInt(itemNameStr, currentItemValue - 1);
            }
        }

        [ContextMenu("Delete all player prefs")]
        public void ClearAllPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        public override string GetValue(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public override void SetValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public override PlayerEquipment LoadEquipment()
        {
            int armor = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_ARMOR, -1);
            int weapon = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_WEAPON, -1);
            int grenade = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_GRENADE, -1);
            int meleeWeapon = PlayerPrefs.GetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_MELEE_WEAPON, -1);
            
            EquipmentValue armorValue = new EquipmentValue(ItemType.EQUIPMENT_ARMOR, (ItemName) armor);
            EquipmentValue weaponValue = new EquipmentValue(ItemType.EQUIPMENT_WEAPON, (ItemName) weapon);
            EquipmentValue grenadeValue = new EquipmentValue(ItemType.EQUIPMENT_GRENADE, (ItemName) grenade);
            EquipmentValue meleeValue = new EquipmentValue(ItemType.EQUIPMENT_MELEE_WEAPON, (ItemName) meleeWeapon);
            
            return new PlayerEquipment(
                weaponValue,
                armorValue,
                meleeValue,
                grenadeValue
                );
        }

        public override void SaveEquipment(PlayerEquipment equipment)
        {
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_ARMOR, (int) equipment.Armor.ItemName);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_WEAPON, (int) equipment.Weapon.ItemName);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_GRENADE, (int) equipment.Grenade.ItemName);
            PlayerPrefs.SetInt(GameHelper.PlayerPrefsKeys.EQUIPMENT_MELEE_WEAPON, (int) equipment.MeleeWeapon.ItemName);
        }
    }
}
