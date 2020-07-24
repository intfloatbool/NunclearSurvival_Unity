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

        public StationProperties Clone()
        {
            return this.MemberwiseClone() as StationProperties;
        }
    }
}

