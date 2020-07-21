
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
        
        [SerializeField] private EquipmentValue _meleeWeapon;
        public EquipmentValue MeleeWeapon => _meleeWeapon;
        
        [SerializeField] private EquipmentValue _grenade;
        public EquipmentValue Grenade => _grenade;

        public PlayerEquipment(EquipmentValue weapon, EquipmentValue armor, EquipmentValue meleeWeapon, EquipmentValue grenade)
        {
            this._weapon = weapon;
            this._armor = armor;
            this._meleeWeapon = meleeWeapon;
            this._grenade = grenade;
        }
    }
}

