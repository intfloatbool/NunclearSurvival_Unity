
using UnityEngine;

namespace NunclearGame.Player
{
    [System.Serializable]
    public struct PlayerEquipment
    {
        [SerializeField] private EquipmentValue _weapon;
        public EquipmentValue Weapon => _weapon;
        
        [SerializeField] private EquipmentValue _armor;
        public EquipmentValue Armor => _armor;

        [SerializeField] private EquipmentValue _grenade;
        public EquipmentValue Grenade => _grenade;

        public PlayerEquipment(EquipmentValue weapon, EquipmentValue armor, EquipmentValue grenade)
        {
            this._weapon = weapon;
            this._armor = armor;
            this._grenade = grenade;
        }

        public EquipmentValue[] GetAllEquipment()
        {
            return new[] {_weapon, _armor, _grenade};
        }
    }
}

