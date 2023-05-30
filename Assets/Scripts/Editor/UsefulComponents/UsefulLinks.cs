using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEditorDebugHelpers
{
    public class UsefulLinks : MonoBehaviour
    {
        private static readonly string hook_name = "[HOOK_FOLDER].prefab";
        
        [MenuItem("UsefulLinks/Preloaders folder")]
        private static void OpenPreloadersFolder()
        {
            var path = "Assets/Prefabs/Preloader/Preloader.prefab";
            TryPingObjectbyPath(path);
        }

        [MenuItem("UsefulLinks/Prefabs folder")]
        private static void OpenPrefabsFolder()
        {
            var path = "Assets/Prefabs/" + hook_name;
            TryPingObjectbyPath(path);
        }
        
        [MenuItem("UsefulLinks/Scripts folder")]
        private static void OpenScriptsFolder()
        {
            var path = "Assets/Scripts/" + hook_name;
            TryPingObjectbyPath(path);
        }

        private static void TryPingObjectbyPath(string path)
        {
            Object assetInPath = GetAssetByPath(path);
            int id = -1;
            if (assetInPath != null)
            {
                id = assetInPath.GetInstanceID();
            }
            if (id <= -1)
            {
                Debug.LogError($"There is no file at path {path}!");
                return;
            }
            EditorGUIUtility.PingObject(id);
        }

        private static Object GetAssetByPath(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Object>(path);
        }
    } 
}

