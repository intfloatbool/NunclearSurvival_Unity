using SingletonsPreloaders;
using UnityEngine;

namespace GameUtils
{
    public static class GameLocalization 
    {
        public static string Get(string key, SystemLanguage? language = null)
        {
            var systemLanguage = language ?? Application.systemLanguage;
            var localized = TextLocalizer.Instance.GetLocalization(key, systemLanguage);
            if (string.IsNullOrEmpty(localized))
            {
                Debug.LogError($"Cannot get localization by key {key}!");
                return key;   
            }
            return localized;
        }
    } 
}

