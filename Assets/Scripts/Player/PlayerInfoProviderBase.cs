using System;
using NunclearGame.Metro;
using NunclearGame.Player;
using UnityEngine;

namespace Player
{
    public abstract class PlayerInfoProviderBase : MonoBehaviour
    {
        public TimeSpan lastSessionTime
        {
            get;
            protected set;
        }
        
        public abstract string LoadPlayerName();
        public abstract void SetPlayerName(string freshName);
        public abstract PlayerInventory LoadInventory();
        public abstract PlayerValues LoadPlayerValues();
        public abstract void SavePlayerValues(PlayerValues playerValues);
        public abstract string GetValue(string key);
        public abstract void SetValue(string key, string value);

        public abstract string LoadCurrentPlayerStationKey();
        public abstract void SetCurrentPlayerStationKey(string stationKey);
        public abstract void UpdateStationData(string stationKey, StationData properties);
        public abstract StationData LoadStationDataByKey(string stationKey);
        public abstract void RemoveAllMetroData();

        public abstract PlayerEquipment LoadEquipment();
        public abstract void SaveEquipment(PlayerEquipment equipment);
        public abstract void SaveLastTime();
        public abstract TimeSpan? CalculateAbsenceTime();
    }
}
