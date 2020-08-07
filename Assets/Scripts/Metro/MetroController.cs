using System;
using NunclearGame.Static;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroController : MonoBehaviour
    {
        [SerializeField] private StationDialogWindow _stationDialogWindow;

        [Space] [SerializeField] private MetroMapView[] _metroPoints;

        private MetroInitializer _metroInitializer;
        private MetroHolder _metroHolder;
        private MetroMapView _lastMetroMapView;

        private bool _isInteractByClicks = true;
        private int nextStationEnergyRequired;

        public event Action<MetroMapView> OnPlayerTransitionToStation;

        private void Awake()
        {
            _metroInitializer = FindObjectOfType<MetroInitializer>();
            Assert.IsNotNull(_metroInitializer, "_metroInitializer != null");

            _metroHolder = GameHelper.MetroHolder;
            Assert.IsNotNull(_metroHolder, "_metroHolder != null");

            foreach (var point in _metroPoints)
            {
                point.OnClicked += () => OnPlayerClickOnMetro(point);
            }

            InitUI();
        }

        public void OnPlayerClickOnMetro(MetroMapView metroMapView)
        {
            if (IsTransitionAvailable(metroMapView))
            {
                nextStationEnergyRequired = metroMapView.StationProperties.energyRequired;
                StartTransitionToMetroStation(metroMapView);
            }
        }

        private void InitUI()
        {
            _stationDialogWindow.close.onClick.AddListener(HideDialog);
            _stationDialogWindow.enter.onClick.AddListener(OnEnterStation);
            _stationDialogWindow.gameObject.SetActive(false);
        }

        private void OnEnterStation()
        {
            GlobalPlayer.PlayerValuesController.RemoveStamina(nextStationEnergyRequired);
        }

        private void StartTransitionToMetroStation(MetroMapView metroMapView)
        {
            var isClearStation = metroMapView.StationProperties.StationData.IsClear;

            if (isClearStation)
            {
                _metroHolder.SetLastPlayerStation(metroMapView.MetroNameKey);
                _lastMetroMapView = metroMapView;
                OnPlayerTransitionToStation?.Invoke(metroMapView);
                OnEnterStation();
            }
            else
            {
                ShowDialog(metroMapView);
            }

            GameHelper.MetroHolder.PotentialStationToGo = metroMapView.StationProperties;
        }

        //TODO: Entering dialog
        private void ShowDialog(MetroMapView metroMapView)
        {
            _isInteractByClicks = false;

            Debug.Log("Station is not clear. Open transition dialog.");

            _stationDialogWindow.UpdateStationDialogByMapView(metroMapView);
            _stationDialogWindow.gameObject.SetActive(true);
        }

        private void HideDialog()
        {
            _stationDialogWindow.gameObject.SetActive(false);
            _isInteractByClicks = true;
        }

        private bool IsTransitionAvailable(MetroMapView metroMapView)
        {
            if (!_isInteractByClicks)
            {
                return false;
            }

            //TODO: Complete transition logic, checking for clearing and so on..
            var currentPlayerStation = _metroInitializer.CurrentPlayerStation;

            if (metroMapView == currentPlayerStation)
            {
                return false;
            }

            var isNearStation = currentPlayerStation.IsRelationStation(metroMapView);

            if (!isNearStation)
            {
                Debug.Log("Station is too far.");

                return false;
            }

            return true;
        }
    }
}