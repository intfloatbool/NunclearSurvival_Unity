using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NunclearGame.BonfireSceneUI
{
    public class InfoElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        public TextMeshProUGUI TitleText => _titleText;
        
        [SerializeField] private TextMeshProUGUI _valueText;
        public TextMeshProUGUI ValueText => _valueText;
        [SerializeField] private Image _iconImg;

        [SerializeField] private Image _filledImg;
        public Image FilledImg => _filledImg;
        public void Init(string titleContent, string valueContent, Sprite iconSprite = null)
        {
            if (_titleText != null)
            {
                _titleText.text = titleContent;
            }

            if (_valueText != null)
            {
                _valueText.text = valueContent;
            }

            if (_iconImg != null)
            {
                if (iconSprite != null)
                {
                    _iconImg.sprite = iconSprite;
                }
            }
        }
    }
}
