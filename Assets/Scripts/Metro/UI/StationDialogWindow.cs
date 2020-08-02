using GameUtils;
using NunclearGame.Enums;
using NunclearGame.Static;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NunclearGame.Metro
{
    public class StationDialogWindow : MonoBehaviour
    {
        public Button enter;
        public Button close;
        public Button background;
        
        [SerializeField] private Image _dangerImg;
        [SerializeField] private TextMeshProUGUI _stationNameValueText;
        [SerializeField] private TextMeshProUGUI _dangerValueText;

        private void Awake()
        {
            Assert.IsNotNull(_stationNameValueText, "_stationNameValueText != null");
            Assert.IsNotNull(_dangerImg, "_dangerImg != null");
            Assert.IsNotNull(_dangerValueText, "_dangerValueText != null");
        }

        public void UpdateStationDialogByMapView(MetroMapView metroMapView)
        {
            DangerType dangerType = metroMapView.StationProperties.DangerType;
            var dangerIconInfo = GameHelper.MetroHolder.GetDangerIconByType(dangerType);

            if (dangerIconInfo != null)
            {
                _dangerImg.sprite = dangerIconInfo.Sprite;
                _dangerImg.color = dangerIconInfo.Color;
            }

            _dangerValueText.text = GameLocalization.Get(dangerType.ToString());

            _stationNameValueText.text = GameLocalization.Get(metroMapView.MetroNameKey);
        }
    }
}