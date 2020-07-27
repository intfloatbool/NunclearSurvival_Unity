using NunclearGame.Static;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Player
{
    public class PlayerViewInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        private void Awake()
        {
            if (_playerView == null)
                _playerView = GetComponent<PlayerView>();
            
            Assert.IsNotNull(_playerView, "_playerView != null");

            if (GameHelper.GlobalPlayer != null)
            {
                GameHelper.GlobalPlayer.EquipmentController.OnEquipmentChanged += UpdateEquipment;
            }
        }

        private void Start()
        {
            InitItems();
        }

        private void OnDestroy()
        {
            if (GameHelper.GlobalPlayer != null)
            {
                GameHelper.GlobalPlayer.EquipmentController.OnEquipmentChanged -= UpdateEquipment;
            }
        }

        private void UpdateEquipment(PlayerEquipment playerEquipment)
        {
            var allEquipment = playerEquipment.GetAllEquipment();
            for (int i = 0; i < allEquipment.Length; i++)
            {
                var equipment = allEquipment[i];
                if (allEquipment[i].ItemName == ItemName.NONE)
                {
                    _playerView.UnequipItemByType(equipment.ItemType);
                }
                var equipmentItemInfo = GameHelper.EquipmentHolder.GetPlayerEquipmentByType(equipment.ItemType);
                if (equipmentItemInfo != null)
                {
                    _playerView.EquipItemByType(equipmentItemInfo.ItemType, equipmentItemInfo.RelativePrefab);
                }
            }
        }

        private void InitItems()
        {
            var itemViews = _playerView.GetViewItems();
            for (int i = 0; i < itemViews.Length; i++)
            {
                var itemView = itemViews[i];
                if(itemView == null)
                    continue;
                var equipmentItemInfo = GameHelper.EquipmentHolder.GetPlayerEquipmentByType(itemView.ItemType);
                if (equipmentItemInfo != null)
                {
                    _playerView.EquipItemByType(equipmentItemInfo.ItemType, equipmentItemInfo.RelativePrefab);
                }

            }
        }
    }
}
