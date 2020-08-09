using System.Collections.Generic;
using Common.Interfaces;
using GameUtils;
using NunclearGame.Player;
using Player;
using SingletonsPreloaders;
using UnityEngine;

namespace NunclearGame.Static
{
    public static class GameHelper
    {
        //Useful managers links
        public static TextLocalizer TextLocalizer => TextLocalizer.Instance;
        public static MetroHolder MetroHolder => MetroHolder.Instance;
        public static PlayerInfoProviderBase InfoProvider => GlobalPlayer?.PlayerInfoProvider;
        public static GlobalPlayer GlobalPlayer => GlobalPlayer.Instance;

        public static EquipmentHolder EquipmentHolder => EquipmentHolder.Instance;
        public static CommonGui CommonGui => CommonGui.Instance;

        public static class AnimationKeys
        {
            public static class PlayerAnimationKeys
            {
                public const string IS_PLAYER_HAVE_WEAPON = "IsHaveWeapon";
                public const string IS_AIM = "IsAim";
                public const string IS_DEAD_BOOL = "IsDead";
                public const string HAND_PUNCH_TRIGGER = "HandPunch";
            }

            /// <summary>
            /// Default keys from common animator for some monsters
            /// </summary>
            public static class DefaultAnimationsKeys
            {
                public const string IS_DEAD_BOOL = "IS_DEAD";
                public const string ATTACK_0_TRIGGER = "ATTACK_0";
                public const string ATTACK_1_TRIGGER = "ATTACK_1";
                public const string HIT_TRIGGER = "HIT";

                /// Animation clip names
                public const string ANIM_ATTACK_0 = "ATTACK_0";
                public const string ANIM_ATTACK_1 = "ATTACK_1";
                public const string ANIM_HIT = "HIT";
                public const string ANIM_DEATH = "DEATH";

                public static string GetAttackClipByTrigger(string triggerName)
                {
                    if (triggerName.Equals(ATTACK_0_TRIGGER))
                        return ANIM_ATTACK_0;
                    
                    return ANIM_ATTACK_1;
                }
                
                private static string[] _attackTriggers;
                public static string GetRndAttackTriggerKey()
                {
                    if (_attackTriggers == null)
                    {
                        _attackTriggers = new[]
                        {
                            ATTACK_0_TRIGGER,
                            ATTACK_1_TRIGGER
                        };
                    }

                    return _attackTriggers[Random.Range(0, _attackTriggers.Length)];
                }
            }
        }

        public static class InputKeys
        {
            public const string MOUSE_X = "Mouse X";
            public const string MOUSE_Y = "Mouse Y";
        }
        
        public static class GameTags
        {
            public const string PLAYER_TAG = "Player";
            public const string METRO_ENEMY_TAG = "MetroEnemy";
        }
        
        public static class ItemValueKeys
        {
            public const string FOOD_NUTRITIONAL = "foodNutritional";
            public const string WATER_THIRST = "waterThirstDrink";
            public const string WEAPON_DAMAGE = "weaponDamage";
            public const string ARMOR = "armorValue";
        }
        
        public static class LocKeys
        {
            public const string NEW_ITEM_DIALOG_HEADER_KEY = "_newItemDialogHeader";
            public const string OKAY_LABEL_KEY = "_okayText";
            public const string DANGER_TEXT = "_dangerText";
        }
        
        public static class PlayerPrefsKeys
        {
            public const string HAS_VALUES_KEY = "p_valuesHasValues";
            public const string PLAYER_LEVEL_KEY = "p_valuesPlayerLevel";
            public const string MAX_HP_KEY = "p_valuesMaxHp";
            public const string CURRENT_HP_KEY = "p_valuesCurrentHp";
            public const string MAX_STAMINA_KEY = "p_valuesMaxStamina";
            public const string RATING_KEY = "p_valuesRating";

            public const string EQUIPMENT_WEAPON = "_equipWeapon";
            public const string EQUIPMENT_ARMOR = "_equipArmor";
            public const string EQUIPMENT_GRENADE = "_equipGrenade";
            
            //Metro stations
            private const string STATION_IS_CLEAR = "isClearKey";
            public static string GetIsClearStationKey(string stationKey)
            {
                return stationKey + STATION_IS_CLEAR;
            }

            public const string PLAYER_LAST_STATION_KEY = "_playerLastStationKey";

        }
        
        public static class PlayerHelper
        {
            public static PlayerValues CreateDefaultValues()
            {
                int defaultPlayerLvl = 1;
                int defaultMaxHp = 175;
                int defaultCurrentHp = defaultMaxHp;
                int defaultMaxStamina = 150;
                int defaultRating = 0;
                return new PlayerValues(
                    defaultPlayerLvl,
                    defaultMaxHp,
                    defaultCurrentHp,
                    defaultMaxStamina,
                    defaultRating
                    );
            }
        }

        public static class HelpFuncs
        {
            public static Vector3 WorldToUISpace(Camera cam,Canvas parentCanvas, Vector3 worldPos)
            {
                //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
                Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
                Vector2 movePos;

                //Convert the screenpoint to ui rectangle local point
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
                //Convert the local point to world point
                return parentCanvas.transform.TransformPoint(movePos);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCollection">object that will added in dict by T1 key</param>
        /// <typeparam name="T1">Primary key of T2 object</typeparam>
        /// <typeparam name="T2">Object reference</typeparam>
        /// <returns></returns>
        public static Dictionary<T1, T2> InitDictionaryByCollection<T1, T2>(IEnumerable<T2> sourceCollection) where T2 : IKeyPreferer<T1>
        {
            var dict = new Dictionary<T1, T2>();

            foreach (var sourceItem in sourceCollection)
            {
                if (sourceItem == null)
                {
                    Debug.LogError("Missing item!");
                    continue;
                }

                var itemKey = sourceItem.GetKey();
                if (dict.ContainsKey(itemKey))
                {
                    Debug.LogError("Dict already has key!");
                }
                else
                {
                    dict.Add(itemKey, sourceItem);   
                }
            }

            return dict;
        }
    }

}