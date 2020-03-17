using System;
using Player;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class NameDialog : Dialog
    {
        [SerializeField] private int _minSymbols = 4;
        [SerializeField] private TMP_InputField _input;
        public override event Action OnDialogDone = () => { };

        public override void DialogDone()
        {
            var inputValue = _input.text;
            if (!string.IsNullOrEmpty(inputValue) && inputValue.Length >= _minSymbols)
            {
                GlobalPlayer.Instance.PlayerNickName = inputValue;
                OnDialogDone();
            }
        }
    }
}