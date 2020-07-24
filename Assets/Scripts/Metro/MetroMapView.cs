using System;
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
        public string MetroNameKey => _metroNameKey;

        [SerializeField] private TextMeshPro _nameText;
        [SerializeField] private TextMeshPro _dangerText;

        [SerializeField] private ColorChangerBase[] _relativeColorChangersByDanger;
        private StationProperties _stationProperties;
        private string _localizedName;

        private void Awake()
        {
            Assert.IsNotNull(_nameText, "_nameText != null");
            Assert.IsNotNull(_dangerText, "_dangerText != null");
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
            _dangerText.text = GameLocalization.Get(_stationProperties.DangerType.ToString());

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
                if(colorChanger == null)
                    continue;
                colorChanger.SetColor(colorByDanger);
                    
            }
        }
    }
}
