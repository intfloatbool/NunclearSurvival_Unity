using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class PlayerGameUnit : GameUnit
    {
        protected override void Awake()
        {
            base.Awake();
            Assert.IsNotNull(GameHelper.GlobalPlayer, "GameHelper.GlobalPlayer != null");
            if (GameHelper.GlobalPlayer != null)
            {
                PlayerValues playerValues = GameHelper.GlobalPlayer.PlayerValues;
                _currentHp = playerValues.CurrentHp;
                _maxHp = playerValues.MaxHp;
            }
        }
    }

}
