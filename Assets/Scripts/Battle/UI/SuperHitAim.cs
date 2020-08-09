using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NunclearGame.Battle.UI
{
    public class SuperHitAim : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TransformScaler _transformScaler;
        [SerializeField] private Vector3 _aimDoneScale = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] private Vector3 _aimStartScale = new Vector3(5f, 5f, 5f);
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private RectTransform _aimVisualRoot;
        [SerializeField] private Button _targetButton;
        [SerializeField] private Image _hitReadyImg;
        [SerializeField] private float _aimingSpeed = 6f;
        
        [Space(5f)]
        [Header("Runtime")]
        [SerializeField] private bool _isHitReady;
        
        private HitTarget _currentTarget;
        
        public bool IsOnProcess { get; private set; }
        
        
        /// <summary>
        /// arg#0 HitTarget = target to attack
        /// arg#1 bool = is aiming done
        /// </summary>
        public event Action<HitTarget, bool> OnAimDoneTargeting;

        private void Awake()
        {
            Assert.IsNotNull(_transformScaler, "_transformScaler != null");
            Assert.IsNotNull(_aimVisualRoot, "_aimVisualRoot != null");
            Assert.IsNotNull(_hitReadyImg, "_hitReadyImg != null");
            Assert.IsNotNull(_targetButton, "_hitReadyImg != null");
            Assert.IsNotNull(_canvas, "_canvas != null");

            if (_targetCamera == null)
            {
                _targetCamera = Camera.main;
            }
            
            Assert.IsNotNull(_targetCamera, "_targetCamera != null");


            if (_targetButton != null)
            {
                _targetButton.onClick.AddListener(StopTargeting);
            }
        }

        private void Start()
        {
            if(_aimVisualRoot.gameObject.activeInHierarchy)
                _aimVisualRoot.gameObject.SetActive(false);
        }

        public void StartTargeting(HitTarget hitTarget)
        {
            _currentTarget = hitTarget;
            _transformScaler.CurrentScale = _aimDoneScale;
            _aimVisualRoot.gameObject.SetActive(true);
            _isHitReady = false;
            IsOnProcess = true;
        }

        public void StopTargeting()
        {
            OnAimDoneTargeting?.Invoke(_currentTarget, _isHitReady);
            _transformScaler.CurrentScale = _aimStartScale;
            _transformScaler.transform.localScale = _aimStartScale;
            _aimVisualRoot.gameObject.SetActive(false);
            _hitReadyImg.gameObject.SetActive(false);
            _currentTarget = null;
            _isHitReady = false;
            IsOnProcess = false;
        }

        private void MoveToTargetLoop()
        {
            if (_targetCamera == null)
                return;
            var uiPos = GameHelper.HelpFuncs.WorldToUISpace(_targetCamera, _canvas, _currentTarget.transform.position);
            _aimVisualRoot.transform.position = uiPos;
        }
        
        private void Update()
        {
            if(_currentTarget == null)
                return;

            MoveToTargetLoop();
            
            _hitReadyImg.gameObject.SetActive(_isHitReady);
            if (_transformScaler.transform.localScale == _aimDoneScale)
            {
                _isHitReady = true;
            }
        }
    } 
}

