using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class TextLocalizer : UnitySingletonBase<TextLocalizer>
    {
        [SerializeField] private string _localizationJsonPath = "Localization/Loc_1";
        [SerializeField] private LocalizationItemsContainer _localizationItemContainer;
        
        private Dictionary<string, LocalizationItem> _localizationItemsDict = new Dictionary<string, LocalizationItem>();
        
        
        protected override TextLocalizer GetInstance()
        {
            return this;
        }

        protected override void Awake()
        {
            base.Awake();
            LoadLocalizationFromFile();
            InitDict();
        }
        

        private void LoadLocalizationFromFile()
        {
            var jsonText = Resources.Load(_localizationJsonPath) as TextAsset;
            Debug.Assert(jsonText != null, "jsonText != null");
            if (jsonText != null)
            {
                _localizationItemContainer = JsonUtility.FromJson<LocalizationItemsContainer>(jsonText.text);
            }
        }

        private void InitDict()
        {
            foreach (var item in _localizationItemContainer.LocalizationItems)
            {
                var key = item.Key;
                if (_localizationItemsDict.ContainsKey(key))
                {
                    Debug.LogWarning($"Loc Key {key} duplicate! Skip.");
                }
                else
                {
                    _localizationItemsDict.Add(key, item);
                    item.InitDict();
                }
            }
        }

        public string GetLocalization(string key, SystemLanguage language)
        {
            var item = _localizationItemsDict.ContainsKey(key) ? _localizationItemsDict[key] : null;
            if (item != null)
            {
                var text = item.GetTextByRegion(language);
                return text;
            }
            Debug.LogError($"Cannot get localization by  key: {key}! Not exists!");
            return key;
        }
    }
}
