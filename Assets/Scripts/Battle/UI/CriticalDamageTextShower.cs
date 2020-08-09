using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using GameUtils;
using NunclearGame.Static;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class CriticalDamageTextShower : MonoBehaviour
    {
        [SerializeField] private UnitsSpawner _unitsSpawner;
        [SerializeField] private TransformScaler _textScaler;
        [SerializeField] private string _locTextKey = "criticalDamageText";
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private float _critTextLifeTime = 1.5f;

        private Coroutine _textShowingCoroutine;
        
        private WaitForSeconds _waiting;
        private Targetable _enemyTargetable;
        private PlayerTapAttackController _playerTapAttackController;
        private UnitDamage _playerUnitDamage;
        
        private void Awake()
        {
            _waiting = new WaitForSeconds(_critTextLifeTime);
            
            Assert.IsNotNull(_unitsSpawner, "_unitsSpawner != null");
            Assert.IsNotNull(_textScaler, "_textScaler != null");
            Assert.IsNotNull(_textMeshPro, "_textMeshPro != null");

            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned += OnUnitSpawned;
            }
        }

        private void OnDestroy()
        {
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned -= OnUnitSpawned;
            }

            if (_playerUnitDamage != null)
            {
                _playerUnitDamage.OnCriticalDamage -= OnPlayerCrit;
            }
        }

        private void OnPlayerCrit(int critDamage)
        {
            if (_textShowingCoroutine != null)
                return;
            
            string locText = GameLocalization.Get(_locTextKey);
            string fullText = $"{locText} {critDamage}!";
            HitTarget lastHitTarget = _enemyTargetable.LastAffectedTarget;
            _textMeshPro.text = fullText;

            if (_textScaler == null)
            {
                Debug.LogError("TextScaler is missing!");
                return;
            }

            if (lastHitTarget == null)
            {
                Debug.LogError("LastHitTarget is missing!");
                return;
            }
            _textScaler.transform.position = new Vector3(
                _textScaler.transform.position.x,
                lastHitTarget.transform.position.y,
                _textScaler.transform.position.z
                );
            
            _textShowingCoroutine = StartCoroutine(ShowCritHighligthTextForTime());

        }

        private IEnumerator ShowCritHighligthTextForTime()
        {
            _textScaler.Show();
            yield return _waiting;
            _textScaler.Hide();
            _textShowingCoroutine = null;
        }

        private void OnUnitSpawned(GameUnit gameUnit)
        {
            if (gameUnit.tag.Equals(GameHelper.GameTags.PLAYER_TAG))
            {
                _playerTapAttackController = gameUnit.GetComponent<PlayerTapAttackController>();
                if (_playerTapAttackController != null)
                {
                    _playerUnitDamage = _playerTapAttackController.UnitDamage;
                    Assert.IsNotNull(_playerUnitDamage, "_playerUnitDamage != null");

                    if (_playerUnitDamage != null)
                    {
                        _playerUnitDamage.OnCriticalDamage += OnPlayerCrit;
                    }
                }
            }
            else if (gameUnit.tag.Equals(GameHelper.GameTags.METRO_ENEMY_TAG))
            {
                _enemyTargetable = gameUnit.GetComponent<Targetable>();
                Assert.IsNotNull(_enemyTargetable, "_enemyTargetable != null");
            }
        }
    }
}

