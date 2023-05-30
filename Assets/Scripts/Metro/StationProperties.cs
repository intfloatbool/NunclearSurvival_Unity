using NunclearGame.Enums;
using UnityEngine;

namespace NunclearGame.Metro
{
    [System.Serializable]
    public class StationProperties
    {
        [SerializeField] private string _name;
        public string Name => _name;
        
        [SerializeField] private DangerType _dangerType;
        public DangerType DangerType => _dangerType;

        [Space(5f)] [Header("Runtime data will loading and update after start!")] 
        [SerializeField] private StationData _stationData;
        public StationData StationData
        {
            get => _stationData;
            set => _stationData = value;
        }
    }
}

