using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class DialogButton : MonoBehaviour
    {
        public bool IsActivated { get; set; }

        [SerializeField] private Button _btn;
        public Button Btn => _btn;

        [SerializeField] private Text _text;
        public void SetText(string textStr)
        {
            _text.text = textStr;
        }

        [SerializeField] private Image _btnImage;
        public void SetColor(Color color)
        {
            _btnImage.color = color;
        }
    }
}

