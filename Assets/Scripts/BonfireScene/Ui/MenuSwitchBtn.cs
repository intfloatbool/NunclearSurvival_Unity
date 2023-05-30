using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NunclearGame
{
    [RequireComponent(typeof(Button))]
    public class MenuSwitchBtn : MonoBehaviour
    {
        [SerializeField] private GameObject _relativeMenu;
        [SerializeField] private GameObjectsSwitcher _switcher;
        private Button _btn;
        private void Awake()
        {
            _btn = GetComponent<Button>();
            Assert.IsNotNull(_btn, "_btn != null");
            
            if (_switcher == null)
            {
                _switcher = FindObjectOfType<GameObjectsSwitcher>();
            }
            
            Assert.IsNotNull(_switcher, "_switcher != null");
            
            Assert.IsNotNull(_relativeMenu, "_relativeMenu != null");
            
            if (_switcher != null)
            {
                if (_btn != null)
                {
                    if (_relativeMenu != null)
                    {
                        _btn.onClick.AddListener(() => _switcher.OpenMenuAndCloseAnother(_relativeMenu));
                    }
                }
                
            }
        }
    }
}
