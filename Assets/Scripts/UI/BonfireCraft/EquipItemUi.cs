using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NunclearGame.BonfireSceneUI
{
    public class EquipItemUi : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        public ItemType ItemType => _itemType;
        [SerializeField] private Image _itemIcon;

        [SerializeField] private Button _btn;
        private void Awake()
        {
            Assert.IsNotNull(_itemIcon, "_itemIcon != null");
            Assert.IsNotNull(_btn, "_btn != null");
            if (_btn != null)
            {
                _btn.onClick.AddListener(OnBtnClick);
            }
        }

        private void OnBtnClick()
        {
            Debug.Log("Unequip item by btn click!");
            GlobalPlayer.PlayerEquipmentController.UnEquip(_itemType);
        }

        public void SetActive(bool isActive)
        {
            if (_itemIcon != null)
            {
                _itemIcon.gameObject.SetActive(isActive);
            }

            if (_btn != null)
            {
                _btn.interactable = isActive;
            }
        }
        
        public void InitItem(Sprite itemSprite = null)
        {
            _itemIcon.sprite = itemSprite;
        }
    }
}

