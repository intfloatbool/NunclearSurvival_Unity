using Common.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class EquipViewItem : MonoBehaviour, IKeyPreferer<ItemType>
    {
        [SerializeField] protected ItemType _itemtype;
        public ItemType ItemType => _itemtype;
        [SerializeField] protected Transform _anchor;
        [SerializeField] protected GameObject _currentItemInstance;
        
        protected virtual void Awake()
        {
            if (_anchor == null)
            {
                _anchor = transform;
            } 
            Assert.IsNotNull(_anchor, "_anchor == null");
        }

        public virtual void SetItem(GameObject itemPrefab)
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
