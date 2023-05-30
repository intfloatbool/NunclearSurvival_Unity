using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class UnitSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameUnit _gameUnit;
        [SerializeField] private UnitDamage _unitDamage;

        [Space(5f)]
        [Header("Clips")]
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private AudioClip _damagedSound;
        [SerializeField] private AudioClip _attackSound;

        private void Awake()
        {
            Assert.IsNotNull(_audioSource, "_audioSource != null");
            Assert.IsNotNull(_gameUnit, "_gameUnit != null");
            Assert.IsNotNull(_unitDamage, "_unitDamage != null");

            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged += OnUnitDamaged;
                _gameUnit.OnDead += OnUnitDeath;
            }

            if (_unitDamage != null)
            {
                _unitDamage.OnAttackTarget += OnUnitAttack;
            }
        }

        private void OnDestroy()
        {
            if (_gameUnit != null)
            {
                _gameUnit.OnDamaged -= OnUnitDamaged;
                _gameUnit.OnDead -= OnUnitDeath;
            }

            if (_unitDamage != null)
            {
                _unitDamage.OnAttackTarget -= OnUnitAttack;
            }
        }

        private void OnUnitDamaged(int dmg)
        {
            PlaySound(_damagedSound);
        }

        private void OnUnitDeath()
        {
            PlaySound(_deathSound);
        }

        private void OnUnitAttack(int dmg, GameUnit target)
        {
            PlaySound(_attackSound);
        }

        private void PlaySound(AudioClip sound)
        {
            if (sound == null)
                return;
            if (_audioSource == null)
                return;
            _audioSource.PlayOneShot(sound);
        }
    }
}

