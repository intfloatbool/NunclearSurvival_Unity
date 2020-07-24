using System;
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
            
            //TODO: Mark danger with something else but no color! Its for test! 
            if (GameHelper.MetroHolder != null)
            {
                var colorByDanger = GameHelper.MetroHolder.GetColorByDanger(_stationProperties.DangerType);
                _dangerText.color = colorByDanger;
            }
        }
    }
}
