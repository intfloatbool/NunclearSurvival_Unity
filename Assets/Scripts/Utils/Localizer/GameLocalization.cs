using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEngine;

namespace GameUtils
{
    public static class GameLocalization 
    {
        public static string Get(string key, SystemLanguage? language = null)
        {
            if (GameHelper.TextLocalizer == null)
                return key;
            var systemLanguage = language ?? Application.systemLanguage;
            var localized = GameHelper.TextLocalizer.GetLocalization(key, systemLanguage);
            if (string.IsNullOrEmpty(localized))
            {
                return key;   
            }
            return localized;
        }
    } 
}

