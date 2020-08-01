using NunclearGame.Static;

namespace NunclearGame.Battle
{
    public class AutoTargetForMetroEnemy : AutoDamageTargetSearcherBase
    {
        protected override string TargetTag => GameHelper.GameTags.METRO_ENEMY_TAG;
    }
}
