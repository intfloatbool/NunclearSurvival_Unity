using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _fireTransformPoint;
        public Transform FireTransformPoint => _fireTransformPoint;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hitSoundClip;
        [SerializeField] private Transform _fireEffect;
        
        private void Awake()
        {
            Assert.IsNotNull(_fireTransformPoint, "_fireTransformPoint != null");
        }

        public void WeaponEffectsShow()
        {
            if (_audioSource != null)
            {
                if (_hitSoundClip != null)
                {
                    _audioSource.PlayOneShot(_hitSoundClip);    
                }
            }

            if (_fireEffect != null)
            {
                _fireEffect.gameObject.SetActive(true);
            }
        }
    }
}

