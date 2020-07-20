using GameUtils;
using SingletonsPreloaders;
using StaticHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using NunclearGame.Dialogs;
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
        [SerializeField] private Transform _dialogBacgrkound;
        [SerializeField] private RectTransform _headArea;

        [Space(5f)] 
        [SerializeField] private DialogButton _btnPrefab;

        [Space(5f)]
        [Header("Parents for dialog items")]
        [SerializeField] private Transform _btnParent;

        [Space(5f)] 
        [SerializeField] private ValueUI _valueUIprefab;
        [SerializeField] private Transform _valuesContainer;
        [SerializeField] private Transform _valuesParent;
        
        [Space(5f)]
        [Header("Runtime references")]
        [SerializeField] private List<DialogButton> _buttons = new List<DialogButton>();

        private RectTransform _rectTransform;
        private List<ValueUI> _curreltValues = new List<ValueUI>();

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            CheckReferences();
            HideDialogVoid();
        }

        private void CheckReferences()
        {
            var dialogAssertionMsg = "Dialog assertion: ";
            Debug.Assert(_headerText != null, dialogAssertionMsg + "_headerText != null");
            Debug.Assert(_headerImage != null, dialogAssertionMsg + "_headerImage != null");
            Debug.Assert(_descriptionText != null, dialogAssertionMsg + "_descriptionText != null");
            Debug.Assert(_dialogBacgrkound != null, dialogAssertionMsg + "_dialogBacgrkound != null");
            Debug.Assert(_btnPrefab != null, dialogAssertionMsg + "btnPrefab != null");
            Debug.Assert(_btnParent != null, dialogAssertionMsg + "_btnParent != null");
            Debug.Assert(_rectTransform != null, dialogAssertionMsg + "_rectTransform != null");
            Debug.Assert(_headArea != null, dialogAssertionMsg + "_headArea != null");
        }


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
                if(onClickAction != null)
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

        public CustomDialog ShowValuesContainer()
        {

            SetActiveValuesContainer(true);
            return this;
        }

        public CustomDialog AddValueInContainer(int valueNum, Sprite valueIcon = null)
        {
            var propertyValue = Instantiate(_valueUIprefab, _valuesParent);
            propertyValue.Init(valueNum.ToString(), valueIcon);
            propertyValue.gameObject.SetActive(true);
            _curreltValues.Add(propertyValue);
            return this;
        }

        private void SetActiveValuesContainer(bool isActive)
        {
            _valuesContainer.gameObject.SetActive(isActive);
            if (!isActive)
            {
                _curreltValues.ForEach(v => Destroy(v.gameObject));
                _curreltValues.Clear();
            }
        }

        public CustomDialog ResetDialog()
        {
            ResetButtons();
            SetActiveValuesContainer(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            return this;
        }

        public CustomDialog HideDialog()
        {        
            _dialogRoot.gameObject.SetActive(false);
            _dialogBacgrkound.gameObject.SetActive(false);
            return this;
        }

        public void HideDialogVoid()
        {
            HideDialog();
        }

        public CustomDialog ShowDialog()
        {         
            _dialogRoot.gameObject.SetActive(true);
            _dialogBacgrkound.gameObject.SetActive(true);

            //call 2 times to align layout group + content size filter
            LayoutRebuilder.ForceRebuildLayoutImmediate(_headArea);          
            LayoutRebuilder.ForceRebuildLayoutImmediate(_headArea);

            return this;
        }

        public void ShowAttentionDialog(string bodyTextKey, string headerTextKey = null,
            Sprite icon = null, Action onOkey = null)
        {
            if(string.IsNullOrEmpty(headerTextKey))
            {
                headerTextKey = LocalizationHelpers.ATTENTION_KEY;
            }
            if(icon == null)
            {
                icon = SpritesHolder.Instance.GetSpriteByKey(SpriteNamesHelper.ATTENTION_ICON);
            }
            ResetDialog();
            SetHeader(headerTextKey, icon);
            SetDialogDescription(bodyTextKey);
            AddButton(LocalizationHelpers.OK_LOC_KEY, onOkey, DialogPartType.ACCESS);
            ShowDialog();
        }

        public void ShowConfirmDialog(Action onConfirmAction, string confirmBodyTextKey, string confirmHeaderTextKey = null)
        {
            if(string.IsNullOrEmpty(confirmHeaderTextKey))
            {
                confirmHeaderTextKey = LocalizationHelpers.ARE_YOU_SURE_KEY;
            }
            ResetDialog();
            SetHeader(confirmHeaderTextKey);
            SetDialogDescription(confirmBodyTextKey);
            AddButton(LocalizationHelpers.NO_LOC_KEY, null, CustomDialog.DialogPartType.SIMPLE);
            AddButton(LocalizationHelpers.YES_LOC_KEY, onConfirmAction, CustomDialog.DialogPartType.ACCESS);
            ShowDialog();
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

