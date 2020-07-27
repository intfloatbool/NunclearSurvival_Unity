using Common.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class EquipViewItem : MonoBehaviour, IKeyPreferer<ItemType>
    {
        [SerializeField] private ItemType _itemtype;
        public ItemType ItemType => _itemtype;
        [SerializeField] private Transform _anchor;
        [SerializeField] private GameObject _currentItemInstance;

        private void Awake()
        {
            if (_anchor == null)
            {
                _anchor = transform;
            } 
            Assert.IsNotNull(_anchor, "_anchor == null");
        }

        public void SetItem(GameObject itemPrefab)
        {
            if (itemPrefab == null)
            {
                if (_currentItemInstance != null)
                {
                    Destroy(_currentItemInstance);
                }
                return;
            }
            if (_currentItemInstance != null)
            {
                Destroy(_currentItemInstance);
            }

            _currentItemInstance = Instantiate(itemPrefab, _anchor);
        }

        public ItemType GetKey()
        {
            return _itemtype;
        }
    }
}
