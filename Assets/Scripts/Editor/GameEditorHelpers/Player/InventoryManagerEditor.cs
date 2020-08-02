using SingletonsPreloaders;
using System;
using Player;
using UnityEditor;
using UnityEngine;

namespace GameEditorDebugHelpers
{
    [CustomEditor(typeof(GlobalPlayer))]
    public class InventoryManagerEditor : Editor
    {
        private ItemName _selectedName;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(2f);
            GUILayout.Label("Add custom item to the Inventory");
            _selectedName = (ItemName) EditorGUILayout.EnumPopup("ItemType to add", _selectedName);
            GlobalPlayer globalPlayer = (GlobalPlayer) target;

            if (GUILayout.Button($"Add {_selectedName}"))
            {
                globalPlayer.PlayerInventory.AddItem(_selectedName);
            }

            if (GUILayout.Button($"Add random item"))
            {
                var itemNames = Enum.GetNames(typeof(ItemName));
                var rndItemIndex = UnityEngine.Random.Range(0, itemNames.Length);
                globalPlayer.PlayerInventory.AddItem((ItemName) rndItemIndex);
            }

            GUILayout.Space(5f);
            GUILayout.Label("Player values:");
            GUILayout.Space(2f);

            if (GUILayout.Button($"Increase player level +1"))
            {
                globalPlayer.ValuesController.IncreaseLevel();
            }

            int healValue = 10;

            if (GUILayout.Button($"Damage player with: {healValue}"))
            {
                globalPlayer.ValuesController.AddHealth(healValue);
            }

            if (GUILayout.Button($"Heal player with: {healValue}"))
            {
                globalPlayer.ValuesController.RemoveHealth(healValue);
            }
            
            int staminaValue = 10;

            if (GUILayout.Button($"Decrease stamina with: {-staminaValue}"))
            {
                globalPlayer.ValuesController.RemoveStamina(staminaValue);
            }

            if (GUILayout.Button($"Add stamina with: {staminaValue}"))
            {
                globalPlayer.ValuesController.AddStamina(staminaValue);
            }

            GUILayout.Space(10f);

            if (GUILayout.Button($"Clear data (requires restart)"))
            {
                var prefsProvider = globalPlayer.PlayerInfoProvider as PlayerPrefsInfoProvider;

                if (prefsProvider != null)
                {
                    prefsProvider.ClearAllPrefs();
                }
            }
        }
    }
}