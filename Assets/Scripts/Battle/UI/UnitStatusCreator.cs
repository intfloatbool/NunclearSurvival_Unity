using System;
using System.Collections;
using System.Collections.Generic;
using GameUtils;
using NunclearGame.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle.UI
{
    public class UnitStatusCreator : MonoBehaviour
    {
        [SerializeField] private UnitsSpawner _unitsSpawner;
        [SerializeField] private Transform _statusRoot;
        [SerializeField] private NamedStatusPanel _statusPanelPrefab;
        private void Awake()
        {
            Assert.IsNotNull(_unitsSpawner, "_unitsSpawner != null");
            Assert.IsNotNull(_statusPanelPrefab, "_statusPanelPrefab != null");
            Assert.IsNotNull(_statusRoot, "_statusRoot != null");

            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned += CreateStatusForUnit;
            }
        }

        private void OnDestroy()
        {
            if (_unitsSpawner != null)
            {
                _unitsSpawner.OnUnitSpawned -= CreateStatusForUnit;
            }
        }

        private void CreateStatusForUnit(GameUnit gameUnit)
        {
            if (_statusRoot == null)
                return;
            if (_statusPanelPrefab == null)
                return;
            string localizedName = GameLocalization.Get(gameUnit.NameKey);
            var statusPanel = Instantiate(_statusPanelPrefab, _statusRoot);
            statusPanel.InitPanel(localizedName, gameUnit);
        }
    }
}
