using SingletonsPreloaders;
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
        }
    }
}

