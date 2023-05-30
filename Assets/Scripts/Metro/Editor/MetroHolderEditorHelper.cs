using System.Collections;
using System.Collections.Generic;
using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEditor;
using UnityEngine;

namespace NunclearGame.Metro
{
    [CustomEditor(typeof(MetroHolder))]
    public class MetroHolderEditorHelper : Editor
    {
        private string _stationName = "";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            MetroHolder holder = target as MetroHolder;
            if (holder == null)
                return;
            
            GUILayout.Space(5f);
            GUILayout.Label("Current station: " + holder.PlayerLastStationKey);
            GUILayout.Space(2f);
            GUILayout.Label("Set player station by name");
            _stationName = GUILayout.TextField(_stationName, 35);
            if (GUILayout.Button("Set player station!"))
            {
                holder.SetLastPlayerStation(_stationName);
            }
            
            GUILayout.Space(5f);
            GUILayout.Label("** DANGER ZONE **");
            GUILayout.Space(2f);

            if (GUILayout.Button("Make all stations cleared"))
            {
                holder.MakeAllStationsCleared();
            }
            
            GUILayout.Space(5f);
            if (GUILayout.Button("Clear metro saved data"))
            {
                if (GameHelper.InfoProvider == null)
                    return;
                GameHelper.InfoProvider.RemoveAllMetroData();
            }

        }
    }
}
