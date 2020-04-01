using System;
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
    }
}
