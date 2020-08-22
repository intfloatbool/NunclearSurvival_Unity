using Common.Visual;
using UnityEngine;

namespace NunclearGame.Battle
{
    public class PlayerDamageController : BattlePlayerControllerBase
    {
        [SerializeField] private SplashImageTransparent _bloodEffect;
        
        protected override void OnPlayerSpawn()
        {
            _player.OnDamaged += OnPlayerDamaged;
        }

        private void OnPlayerDamaged(int damage)
        {
            if (_bloodEffect != null)
            {
                _bloodEffect.Show();
            }
        }
    }
}

