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
        [SerializeField] private AudioClip[] _deathSounds;
        [SerializeField] private AudioClip[] _damagedSounds;
        [SerializeField] private AudioClip[] _attackSounds;

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

        private AudioClip GetRandomSound(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }
        
        private void OnUnitDamaged(int dmg)
        {
            if(_damagedSounds.Length <= 0)
                return;
            PlaySound(GetRandomSound(_damagedSounds));
        }

        private void OnUnitDeath()
        {
            if(_deathSounds.Length <= 0)
                return;
            PlaySound(GetRandomSound(_deathSounds));
        }

        private void OnUnitAttack(int dmg, GameUnit target)
        {
            if(_attackSounds.Length <= 0)
                return;
            PlaySound(GetRandomSound(_attackSounds));
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

