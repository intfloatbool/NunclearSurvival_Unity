using System;
using NunclearGame.Player;
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


        private void Awake()
        {
            GlobalPlayer.PlayerValuesController.OnPlayerValuesChanged += InitPlayerValuesView;
        }
        

        private void OnDestroy()
        {
            GlobalPlayer.PlayerValuesController.OnPlayerValuesChanged -= InitPlayerValuesView;
        }

        private void OnEnable()
        {
            var globalPlayer = GlobalPlayer.Instance;
            if (globalPlayer != null)
            {
                InitPlayerValuesView(globalPlayer.PlayerValues);
            }
            
        }

        private void InitPlayerValuesView(PlayerValues playerValues)
        {
            
            if (_levelElement != null)
            {
                _levelElement.ValueText.text = playerValues.PlayerLvl.ToString();
            }

            if (_hpElement != null)
            {
                var fillAmount = (float) playerValues.CurrentHp / (float) playerValues.MaxHp;
                if (_hpElement.FilledImg != null)
                {
                    _hpElement.FilledImg.fillAmount = fillAmount;
                }
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
