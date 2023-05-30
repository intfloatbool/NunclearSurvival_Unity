using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroStationDialog : MonoBehaviour
    {
        [SerializeField] private StationDialog _stationDialog;
        private MetroController _metroController;

        private void Awake()
        {
            _metroController = FindObjectOfType<MetroController>();
            
            Assert.IsNotNull(_metroController, "_metroController != null");
            
            Assert.IsNotNull(_stationDialog, "_stationDialog != null");

            if (_metroController != null)
            {
                _metroController.OnPlayerTryEnterStation += ShowDialog;
            }
        }
        
        private void OnDestroy()
        {
            if (_metroController != null)
            {
                _metroController.OnPlayerTryEnterStation -= ShowDialog;
            }
        }

        private void Start()
        {
            _stationDialog.gameObject.SetActive(false);
        }

        private void ShowDialog(MetroMapView metroMapView)
        {
            _metroController.IsInteractByClicks = false;
            _stationDialog.gameObject.SetActive(true);
            _stationDialog.UpdateStationDialogByMapView(metroMapView);
        }

        public void HideDialog()
        {
            _stationDialog.gameObject.SetActive(false);
            _metroController.IsInteractByClicks = true;
        }
    }
}
