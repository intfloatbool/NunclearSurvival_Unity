using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NunclearGame.Dialogs
{
    public class ValueUI : MonoBehaviour
    {
        [SerializeField] private Image _img;
        [SerializeField] private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            Assert.IsNotNull(_img, "_img != null");
            Assert.IsNotNull(_textMesh, "_textMesh != null");
        }

        public void Init(string valueText, Sprite valueIcon = null)
        {
            _textMesh.text = valueText;
            _img.sprite = valueIcon;
        }
    }
}
