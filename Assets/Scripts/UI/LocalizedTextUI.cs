using System;
using GameUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class LocalizedTextUI : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUgui;
        private Text _textUi;
        [SerializeField] private string _textKey;
        [SerializeField] private bool _isSystemLanguageUsing = true;
        [SerializeField] private SystemLanguage _languageToLoad = SystemLanguage.English;

        private void Awake()
        {
            string text = _isSystemLanguageUsing
                ? GameLocalization.Get(_textKey)
                : GameLocalization.Get(_textKey, _languageToLoad);
            _textMeshProUgui = GetComponent<TextMeshProUGUI>();
            if (_textMeshProUgui != null)
            {
                _textMeshProUgui.text = text;  
            }

            if (_textMeshProUgui == null)
            {
                _textUi = GetComponent<Text>();
                if (_textUi != null)
                {
                    _textUi.text = text;
                }
            }
            
        }
    }
}

