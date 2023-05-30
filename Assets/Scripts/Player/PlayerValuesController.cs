using System;
using SingletonsPreloaders;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerValuesController
    {
        private GlobalPlayer _globalPlayer;

        public PlayerValuesController(GlobalPlayer player)
        {
            _globalPlayer = player;
            Assert.IsNotNull(_globalPlayer, "_globalPlayer != null");
        }

        public event Action<PlayerValues> OnPlayerValuesChanged;
        
        public void AddStamina(int value)
        {
            value = Mathf.Abs(value);
            ChangeStamina(value);
        }
        
        public void RemoveStamina(int value)
        {
            value = Mathf.Abs(value);
            ChangeStamina(-value);
        }
        
        public void AddHealth(int value)
        {
            value = Mathf.Abs(value);
            ChangeHealth(value);
        }
        
        public void RemoveHealth(int value)
        {
            value = Mathf.Abs(value);
            ChangeHealth(-value);
        }

        public void IncreaseLevel()
        {
            var currentValues = _globalPlayer.PlayerValues;

            PlayerValues newValues = new PlayerValues(
                currentValues.PlayerLvl + 1,
                currentValues.MaxHp,
                currentValues.CurrentHp,
                currentValues.MaxStamina,
                currentValues.CurrentStamina,
                currentValues.Rating
            );

            _globalPlayer.PlayerValues = newValues;

            OnPlayerValuesChanged?.Invoke(newValues);
        }

        private void ChangeHealth(int value)
        {
            var currentValues = _globalPlayer.PlayerValues;
            int hpAfterChanges = currentValues.CurrentHp + value;
            hpAfterChanges = Mathf.Clamp(hpAfterChanges, 0, currentValues.MaxHp);

            PlayerValues newValues = new PlayerValues(
                currentValues.PlayerLvl,
                currentValues.MaxHp,
                hpAfterChanges,
                currentValues.MaxStamina,
                currentValues.CurrentStamina,
                currentValues.Rating
            );

            _globalPlayer.PlayerValues = newValues;

            OnPlayerValuesChanged?.Invoke(newValues);
        }

        private void ChangeStamina(int value)
        {
            var currentValues = _globalPlayer.PlayerValues;
            int staminaAfterChanges = currentValues.CurrentStamina + value;
            staminaAfterChanges = Mathf.Clamp(staminaAfterChanges, 0, currentValues.MaxStamina);

            PlayerValues newValues = new PlayerValues(
                currentValues.PlayerLvl,
                currentValues.MaxHp,
                currentValues.CurrentHp,
                currentValues.MaxStamina,
                staminaAfterChanges,
                currentValues.Rating
            );

            _globalPlayer.PlayerValues = newValues;

            OnPlayerValuesChanged?.Invoke(newValues);
        }
    }
}