using UnityEngine;
using UnityEngine.UI;

namespace NunclearGame.BonfireSceneUI
{
    public class EquipItemUi : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        public ItemType ItemType => _itemType;
        [SerializeField] private Image _itemIcon;
        
        public void InitItem(Sprite itemSprite = null)
        {
            _itemIcon.gameObject.SetActive(itemSprite != null);
            _itemIcon.sprite = itemSprite;
        }
    }
}

