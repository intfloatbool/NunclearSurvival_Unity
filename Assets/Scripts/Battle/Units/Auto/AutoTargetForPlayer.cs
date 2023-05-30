using NunclearGame.Static;

namespace NunclearGame.Battle
{
    public class AutoTargetForPlayer : AutoDamageTargetSearcherBase
    {
        protected override string TargetTag => GameHelper.GameTags.PLAYER_TAG;
    } 
}

