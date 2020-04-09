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
        [Header("Dialog header properties")]
        [SerializeField] private Text _headerText;
        [SerializeField] private Image _headerImage;
        [SerializeField] private Text _descriptionText;

        [Space(3f)]
        [SerializeField] private Transform _dialogRoot;

        [Space(5f)] 
        [SerializeField] private DialogButton _btnPrefab;

        [Space(5f)]
        [Header("Parents for dialog items")]
        [SerializeField] private Transform _btnParent;

        [Space(5f)]
        [Header("Runtime references")]
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

        public CustomDialog SetHeader(string textKey, Sprite imgSprite = null)
        {
            _headerText.text = GameLocalization.Get(textKey);
            if(imgSprite != null)
            {
                _headerImage.gameObject.SetActive(true);
                _headerImage.sprite = imgSprite;
            }
            else
            {
                _headerImage.gameObject.SetActive(false);
            }
            return this;
        }

        public CustomDialog SetDialogDescription(string descriptionKey)
        {
            _descriptionText.text = GameLocalization.Get(descriptionKey);
            return this;
        }
        

        public CustomDialog AddButton(
                string textKey, 
                Action onClickAction,
                DialogPartType partType = DialogPartType.SIMPLE,
                bool isCloseDialog = true
            )
        {
            var dialogBtn = _buttons.FirstOrDefault(b => !b.IsActivated);

            if(dialogBtn == null)
            {
                dialogBtn = Instantiate(_btnPrefab, _btnParent);
                _buttons.Add(dialogBtn);         
            }
            else
            {
                dialogBtn.gameObject.SetActive(true);
            }

            dialogBtn.IsActivated = true;

            dialogBtn.Btn.onClick.AddListener(() => {
                onClickAction();
                if(isCloseDialog)
                {
                    HideDialog();
                }
            });

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

        private void ResetButtons()
        {
            for(int i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                button.Btn.onClick.RemoveAllListeners();
                button.IsActivated = false;
                button.gameObject.SetActive(false);
            }
        }

    }
}

