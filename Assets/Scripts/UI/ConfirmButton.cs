using SingletonsPreloaders;
using StaticHelpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUI
{
    [RequireComponent(typeof(Button))]
    public class ConfirmButton : MonoBehaviour, IPointerDownHandler
    {
        private Button _btn;
        [SerializeField] private string _confirmHeaderTextKey;
        [SerializeField] private string _confirmBodyTextKey;
        public void OnPointerDown(PointerEventData eventData)
        {
            var dialog = CommonGui.Instance?.GetDialog();
            if(dialog != null)
            {
                dialog.ShowConfirmDialog(InvokeClick, _confirmBodyTextKey, _confirmHeaderTextKey);
            }
        }

        private void InvokeClick()
        {
            _btn.onClick.Invoke();
        }

        private void Awake()
        {
            _btn = GetComponent<Button>();

            var colorBlockCopy = _btn.colors;
            colorBlockCopy.disabledColor = colorBlockCopy.normalColor;
            _btn.colors = colorBlockCopy;

            _btn.interactable = false;
        }

        
    }
}
