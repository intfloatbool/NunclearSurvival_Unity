using System;
using NunclearGame.Player;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Metro
{
    public class MetroCamControl : MonoBehaviour
    {
        [SerializeField] private bool _isEnableMoveByPlayer = true;
        [SerializeField] private float _moveSpeed = 5f;
        
        [SerializeField] private MetroPlayerShower _metroPlayerShower;
        private PlayerView _playerView;
        private void Awake()
        {
            if (_metroPlayerShower == null)
            {
                _metroPlayerShower = FindObjectOfType<MetroPlayerShower>();
            }
            
            Assert.IsNotNull(_metroPlayerShower, "_metroPlayerShower != null");
            if (_metroPlayerShower != null)
            {
                _metroPlayerShower.OnPlayerViewCreated += OnPlayerViewCreated;
                _metroPlayerShower.OnPlayerPositionChanged += OnPlayerPosChanged;
            }
        }

        private void OnDestroy()
        {
            if (_metroPlayerShower != null)
            {
                _metroPlayerShower.OnPlayerViewCreated -= OnPlayerViewCreated;
                _metroPlayerShower.OnPlayerPositionChanged -= OnPlayerPosChanged;
            }
        }

        private void OnPlayerViewCreated(PlayerView playerView)
        {
            _playerView = playerView;
        }

        private void OnPlayerPosChanged(Vector3 playerPos)
        {
            SetMetroCamPosition(playerPos);
        }

        private void SetMetroCamPosition(Vector3 targetPos)
        {
            transform.position = new Vector3(
                targetPos.x,
                transform.position.y,
                targetPos.z
                );
        }

        public void PutCameraToPlayer()
        {
            if (_playerView != null)
            {
                SetMetroCamPosition(_playerView.transform.position);    
            }
        }

        private void Update()
        {
            if (!_isEnableMoveByPlayer)
            {
                return;
            }
            
            if (Input.GetMouseButton(0))
            {
                var mouseX = Input.GetAxis(GameHelper.InputKeys.MOUSE_X);
                var mouseY = Input.GetAxis(GameHelper.InputKeys.MOUSE_Y);
                
                var mouseAxisVector3 = new Vector3(
                    -mouseX,
                    -mouseY,
                    0
                    );
                transform.Translate(mouseAxisVector3 * _moveSpeed * Time.deltaTime);
            }
        }
    } 
}

