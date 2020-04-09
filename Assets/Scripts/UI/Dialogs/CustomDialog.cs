using GameUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class CustomDialog : MonoBehaviour
    {
        public enum DialogPartType
        {
            ACCESS,
            DANGER,
            SIMPLE
        }

        [System.Serializable]
        public struct ColorByPartType
        {
            public DialogPartType DialogPartType;
            public Color Color;
        }

        [SerializeField] private List<ColorByPartType> _colorPartsByType;
        [Space(3f)]
        [Header("Dialog header text")]
        [SerializeField] private Text _headerText;

        [Space(3f)]
        [SerializeField] private Transform _dialogRoot;

        [Space(5f)]
        [SerializeField] private Text _textPrefab;
        [SerializeField] private Image _imgPrefab;
        [SerializeField] private DialogButton _btnPrefab;

        [Space(5f)]
        [Header("Parents for dialog items")]
        [SerializeField] private Transform _textParent;
        [SerializeField] private Transform _imgParent;
        [SerializeField] private Transform _btnParent;

        [Space(5f)]
        [Header("Runtime references")]
        [SerializeField] private List<Text> _texts = new List<Text>();
        [SerializeField] private List<Image> _images = new List<Image>();
        [SerializeField] private List<DialogButton> _buttons = new List<DialogButton>();


        private Color GetColorByDialogPartType(DialogPartType partType)
        {
            if(_colorPartsByType.Any(c => c.DialogPartType == partType))
            {
                return _colorPartsByType.FirstOrDefault(c => c.DialogPartType == partType).Color;
            }
            else
            {
                return Color.black;
            }
        }

        public CustomDialog SetHeader(string textKey)
        {
            _headerText.text = GameLocalization.Get(textKey);
            return this;
        }
        

        public CustomDialog AddButton(string textKey, Action onClickAction, DialogPartType partType = DialogPartType.SIMPLE)
        {
            var dialogBtn = _buttons.FirstOrDefault(b => b != null && b.gameObject.activeInHierarchy == false);
            if(dialogBtn == null)
            {
                dialogBtn = Instantiate(_btnPrefab, _btnParent);           
            }

            dialogBtn.Btn.onClick.AddListener(() => onClickAction());

            dialogBtn.SetText(
                GameLocalization.Get(textKey)
                );

            dialogBtn.SetColor(
                GetColorByDialogPartType(partType)
                );
            
            return this;
        }

        public CustomDialog ResetDialog()
        {
            ResetTexts();
            ResetImages();
            ResetButtons();
            
            return this;
        }

        public CustomDialog HideDialog()
        {
            _dialogRoot.gameObject.SetActive(false);
            return this;
        }

        public CustomDialog ShowDialog()
        {
            _dialogRoot.gameObject.SetActive(true);
            return this;
        }

        private void ResetTexts()
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                var text = _texts[i];
                text.gameObject.SetActive(false);
            }
        }

        private void ResetButtons()
        {
            for(int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                button.Btn.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }
        }

        private void ResetImages()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                var img = _images[i];
                img.gameObject.SetActive(false);
            }
        }

    }
}

