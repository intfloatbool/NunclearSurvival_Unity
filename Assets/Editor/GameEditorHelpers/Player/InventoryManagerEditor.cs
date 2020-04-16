using SingletonsPreloaders;
using System;
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
            GlobalPlayer globalPlayer = (GlobalPlayer)target;
            if(GUILayout.Button($"Add {_selectedName}"))
            {
                globalPlayer.PlayerInventory.AddItem(_selectedName);
            }
            if(GUILayout.Button($"Add random item"))
            {
                var itemNames = Enum.GetNames(typeof(ItemName));
                var rndItemIndex = UnityEngine.Random.Range(0, itemNames.Length);
                globalPlayer.PlayerInventory.AddItem((ItemName) rndItemIndex);
            }
        }
    }
}

