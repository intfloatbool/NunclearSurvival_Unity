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

        // public void AddDamage(int dmg)
        // {
        //     var currentValues = _globalPlayer.PlayerValues;
        //     int hpAfterDamage = currentValues.CurrentHp - dmg;
        //     hpAfterDamage = Mathf.Clamp(hpAfterDamage, 0, currentValues.MaxHp);
        //     
        //     PlayerValues newValues = new PlayerValues(
        //         currentValues.PlayerLvl,
        //         currentValues.MaxHp,
        //         hpAfterDamage,
        //         currentValues.MaxStamina,
        //         currentValues.Rating
        //     );
        //
        //     _globalPlayer.PlayerValues = newValues;
        //     
        //     OnPlayerValuesChanged?.Invoke(newValues);
        // }
        //
        // public void HealUp(int healValue)
        // {
        //     var currentValues = _globalPlayer.PlayerValues;
        //     int hpAfterHeal = currentValues.CurrentHp + healValue;
        //     hpAfterHeal = Mathf.Clamp(hpAfterHeal, 0, currentValues.MaxHp);
        //     
        //     PlayerValues newValues = new PlayerValues(
        //         currentValues.PlayerLvl,
        //         currentValues.MaxHp,
        //         hpAfterHeal,
        //         currentValues.MaxStamina,
        //         currentValues.Rating
        //     ); 
        //     
        //     _globalPlayer.PlayerValues = newValues;
        //     
        //     OnPlayerValuesChanged?.Invoke(newValues);
        // }

        public void ChangeHealth(int value)
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

        public void ChangeStamina(int value)
        {
            var currentValues = _globalPlayer.PlayerValues;
            int staminaAfterChanges = currentValues.CurrentStamina + value;
            staminaAfterChanges = Mathf.Clamp(staminaAfterChanges, 0, currentValues.MaxHp);

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