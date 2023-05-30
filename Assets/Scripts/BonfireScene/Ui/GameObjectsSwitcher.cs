using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame
{
    public class GameObjectsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _allMenu;
        [SerializeField] private GameObject _defaultMenu;

        private void Awake()
        {
            Assert.IsNotNull(_defaultMenu, "_defaultMenu != null");
            if (_defaultMenu != null)
            {
                OpenMenuAndCloseAnother(_defaultMenu);
            }
        }

        public void OpenMenuAndCloseAnother(GameObject targetMenu)
        {
            foreach (var menuGo in _allMenu)
            {
                if (menuGo == targetMenu)
                {
                    menuGo.SetActive(true);
                }
                else
                {
                    menuGo.SetActive(false);
                }
            }
        }
    }
}
