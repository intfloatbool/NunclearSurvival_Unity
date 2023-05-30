using System;
using System.Collections.Generic;
using Common.Visual;
using GameUtils;
using NunclearGame.Static;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroMapView : MonoBehaviour
    {
        [SerializeField] private string _metroNameKey;

        [SerializeField] private TextMeshPro _nameText;

        [SerializeField] private ColorChangerBase[] _relativeColorChangersByDanger;
        [SerializeField] private ColorChangerBase[] _relativeColorChangersByPlayerIsHere;
        [SerializeField] private GameObject[] _clearedRelativeObjects;
        [SerializeField] private GameObject[] _notClearedRelativeObjects;

        [Space(5f)] [SerializeField] private bool _isUseGizmosHelpers = true;
        [SerializeField] private Color _gizmosRelationLinesColor = Color.yellow;
        [Space(5f)] [SerializeField] private List<MetroMapView> _relativeStations;

        private StationProperties _stationProperties;

        private string _localizedName;
        private string _debugName;

        public StationProperties StationProperties => _stationProperties;
        public string MetroNameKey => _metroNameKey;

        public event Action OnClicked;

        private void Awake()
        {
            Assert.IsNotNull(_nameText, "_nameText != null");
        }

        public bool IsRelationStation(MetroMapView metroMapView)
        {
            return _relativeStations.Contains(metroMapView);
        }

        public string GetLocalizedName()
        {
            if (string.IsNullOrEmpty(_localizedName))
            {
                _localizedName = GameLocalization.Get(_metroNameKey);
            }

            return _localizedName;
        }

        public void InitStation(StationProperties properties)
        {
            _stationProperties = properties;
            _nameText.text = GetLocalizedName();
            var stationData = properties.StationData;
            SetIsCleared(stationData.IsClear);
            UpdateColorByDanger();
        }

        private void UpdateColorByDanger()
        {
            if (GameHelper.MetroHolder == null)
            {
                Debug.LogError($"{nameof(GameHelper.MetroHolder)} is MISSING!");

                return;
            }

            Color colorByDanger = GameHelper.MetroHolder.GetColorByDanger(_stationProperties.DangerType);

            for (int i = 0; i < _relativeColorChangersByDanger.Length; i++)
            {
                var colorChanger = _relativeColorChangersByDanger[i];

                if (colorChanger == null)
                {
                    continue;
                }

                colorChanger.SetColor(colorByDanger);
            }
        }

        public void SetPlayerIsHere(bool isPlayerHere)
        {
            if (GameHelper.MetroHolder == null)
            {
                Debug.LogError("Metro holder is missing!");

                return;
            }

            Color placedColor = isPlayerHere
                ? GameHelper.MetroHolder.PlayerHereColor
                : GameHelper.MetroHolder.PlayerNotHereColor;

            for (int i = 0; i < _relativeColorChangersByPlayerIsHere.Length; i++)
            {
                var colorChanger = _relativeColorChangersByPlayerIsHere[i];

                if (colorChanger == null)
                {
                    continue;
                }

                colorChanger.SetColor(placedColor);
            }
        }

        public void SetIsCleared(bool isCleared)
        {
            SetActiveObjectsInCollection(_clearedRelativeObjects, isCleared);
            SetActiveObjectsInCollection(_notClearedRelativeObjects, !isCleared);
        }

        private void SetActiveObjectsInCollection(IEnumerable<GameObject> goCollection, bool isActive)
        {
            foreach (var gameObjectInCol in goCollection)
            {
                gameObjectInCol.SetActive(isActive);
            }
        }

        private void OnDrawGizmos()
        {
            if (!_isUseGizmosHelpers)
            {
                return;
            }

            AutoNameSetting();
            DrawRelationsGizmo();
        }

        [ContextMenu("Reset name")]
        public void ResetDebugName()
        {
            _debugName = null;
        }

        private void AutoNameSetting()
        {
            if (string.IsNullOrEmpty(_debugName))
            {
                _debugName = $"{nameof(MetroMapView)}_{_metroNameKey}";
            }

            if (!gameObject.name.Equals(_debugName))
            {
                gameObject.name = _debugName;
            }
        }

        private void DrawRelationsGizmo()
        {
            Gizmos.color = _gizmosRelationLinesColor;

            foreach (MetroMapView relativeStation in _relativeStations)
            {
                Gizmos.DrawLine(transform.position, relativeStation.transform.position);
            }
        }

        private void OnMouseDown()
        {
            OnClicked?.Invoke();
        }
    }
}