using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class LocalizationItemsContainer
    {
        public LocalizationItem[] LocalizationItems;
    }
    [Serializable]
    public class LocalizationItem
    {
        public string Key;
        public TextByRegion[] TextByRegions;
        private Dictionary<string, string> _languageInfoDict = new Dictionary<string, string>();
        private bool _isDictInitialized = false;
        public void InitDict()
        {
            foreach (var textByRegion in TextByRegions)
            {
                var regionKey = textByRegion.RegionName;
                if (_languageInfoDict.ContainsKey(regionKey))
                {
                    Debug.LogWarning($"Region key: {regionKey} duplicate! In loc key: {Key} , Skip.");
                }
                else
                {
                    _languageInfoDict.Add(regionKey, textByRegion.Text);
                }
            }

            _isDictInitialized = true;
        }

        public string GetTextByRegion(SystemLanguage language)
        {
            if (!_isDictInitialized)
            {
                Debug.LogError("DICT NOT INITIALIZED!");
                return null;
            }
            var languageKey = language.ToString();
            if (_languageInfoDict.ContainsKey(languageKey))
            {
                return _languageInfoDict[languageKey];
            }
        
            return null;
        }
    }
    
    [Serializable]
    public class TextByRegion
    {
        public string RegionName;
        public string Text;
    }
}
