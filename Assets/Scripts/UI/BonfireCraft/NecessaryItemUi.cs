using UnityEngine.UI;
using UnityEngine;
using SingletonsPreloaders;

namespace GameUI 
{
    public class NecessaryItemUi : MonoBehaviour 
    {
        [SerializeField] private ItemType _linkedType;
        public ItemType LinkedType => _linkedType;
        [SerializeField] private Image _image;
        [SerializeField] private string _missingSpriteName = "missing_";

        [SerializeField] private ItemInfo _lastItemInfo;
        public ItemInfo LastItemInfo => _lastItemInfo;
        
        public bool IsReady {get; private set;}

        private void Start()
        {
            ResetItem();
        }
        public void UpdateItem(InventoryItemUi itemUi) 
        {          
            _lastItemInfo = itemUi.CurrentItemInfo.Clone();
            _image.sprite = itemUi.ItemSprite;
            IsReady = true;
        }

        public void ResetItem() 
        {
            _image.sprite = SpritesHolder.Instance.GetSpriteByKey(_missingSpriteName);
            _lastItemInfo = null;
            IsReady = false;
        }
    }
}