using System;
using System.Text;
using NunclearGame.Metro;
using UnityEditor;
using UnityEngine;

namespace GameEditorDebugHelpers
{
    [CustomEditor(typeof(MetroMapView))]
    public class MetroStationHelperEditor : Editor
    {
        StringBuilder _stringBuilder = new StringBuilder();
        private void OnSceneGUI()
        {
            var metroView = target as MetroMapView;
            if (metroView == null)
                return;
            Handles.color = Color.white;
            _stringBuilder.Append("Station Key: ");
            _stringBuilder.Append(metroView.MetroNameKey);
            Handles.Label(metroView.transform.position + Vector3.up * 2,
                _stringBuilder.ToString());
            _stringBuilder.Clear();

        }
    }
}
