using System;
using SingletonsPreloaders;
using UnityEngine;

namespace NunclearGame.BonfireSceneUI
{
    public class PlayerValuesWindow : MonoBehaviour
    {
        [SerializeField] private InfoElement _levelElement;
        [SerializeField] private InfoElement _hpElement;
        [SerializeField] private InfoElement _staminaElement;
        [SerializeField] private InfoElement _rankElement;

        private void OnEnable()
        {
            InitPlayerValuesView();
        }

        private void InitPlayerValuesView()
        {
            var globalPlayer = GlobalPlayer.Instance;
            if (globalPlayer != null)
            {
                var playerValues = globalPlayer.PlayerValues;
                if (_levelElement != null)
                {
                    _levelElement.ValueText.text = playerValues.PlayerLvl.ToString();
                }

                if (_hpElement != null)
                {
                    _hpElement.ValueText.text = $"{playerValues.CurrentHp} / {playerValues.MaxHp}";
                }

                if (_staminaElement != null)
                {
                    _staminaElement.ValueText.text = playerValues.MaxStamina.ToString();
                }

                if (_rankElement != null)
                {
                    _rankElement.ValueText.text = playerValues.Rating.ToString();
                }
            }
        }
    }
}
