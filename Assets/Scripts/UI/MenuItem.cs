using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class MenuItem : MonoBehaviour
    {
        [SerializeField] private Image _img;
        [SerializeField] private Button _btn;

        public event Action<MenuItem> OnClickItem = (menuItem) => { };

        private void Start()
        {
            Debug.Assert(_img != null, "_img != null");
            Debug.Assert(_btn != null, "_btn != null");

            _btn.onClick.AddListener(OnClick);
        }

        public void SetColor(Color color)
        {
            _img.color = color;
        }

        private void OnClick()
        {
            OnClickItem(this);
        }

        public void ImitateClick()
        {
            OnClick();
        }
        

    }
}

