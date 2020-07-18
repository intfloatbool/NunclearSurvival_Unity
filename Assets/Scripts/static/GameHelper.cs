using NunclearGame.Player;

namespace NunclearGame.Static
{
    public static class GameHelper
    {
        public static class LocKeys
        {
            public const string NEW_ITEM_DIALOG_HEADER_KEY = "_newItemDialogHeader";
            public const string OKAY_LABEL_KEY = "_okayText";
        }
        
        public static class PlayerPrefsKeys
        {
            public const string HAS_VALUES_KEY = "p_valuesHasValues";
            public const string PLAYER_LEVEL_KEY = "p_valuesPlayerLevel";
            public const string MAX_HP_KEY = "p_valuesMaxHp";
            public const string CURRENT_HP_KEY = "p_valuesCurrentHp";
            public const string MAX_STAMINA_KEY = "p_valuesMaxStamina";
            public const string RATING_KEY = "p_valuesRating";
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
    }

}