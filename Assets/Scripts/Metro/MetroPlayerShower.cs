using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroPlayerShower : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransformVariant;
        [SerializeField] private MetroPlayerMark _metroPlayerMark;
        [SerializeField] private Vector3 _posOffset = new Vector3(0, 2, 0);
        private PlayerView _playerViewInstance;

        private void Awake()
        {
            Assert.IsNotNull(GameHelper.GlobalPlayer, "GameHelper.GlobalPlayer != null");
            if (GameHelper.GlobalPlayer != null)
            {
                CreatePlayer(GameHelper.GlobalPlayer.PlayerViewPrefab);
            }
            
            Assert.IsNotNull(_metroPlayerMark, "_metroPlayerMark != null");
            if (_metroPlayerMark != null)
            {
                _metroPlayerMark.OnPlayerMarkChanged += PutPlayerAtMetroStation;
            }
        }

        private void OnDestroy()
        {
            if (_metroPlayerMark != null)
            {
                _metroPlayerMark.OnPlayerMarkChanged -= PutPlayerAtMetroStation;
            }
        }

        private void CreatePlayer(PlayerView prefab)
        {
            if (prefab == null)
            {
                Debug.LogError("PlayerView prefab is missing!");
                return;
            }
            _playerViewInstance = Instantiate(prefab);
            if (_playerViewInstance != null &&_playerTransformVariant != null)
            {
                _playerViewInstance.transform.localScale = _playerTransformVariant.localScale;
                _playerViewInstance.transform.rotation = _playerTransformVariant.rotation;
            }
        }

        private void PutPlayerAtMetroStation(MetroMapView mapView)
        {
            if (_playerViewInstance != null)
            {
                var posToPlace = mapView.transform.position + _posOffset;
                _playerViewInstance.transform.position = posToPlace;
            }
        }
    }
}
