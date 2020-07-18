using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NunclearGame.BonfireSceneUI
{
    public class InventoryItemCopyUI : MonoBehaviour
    {
        [SerializeField] private Image _img;
        public ItemInfo ItemInfo { get; private set; }
        public void Init(ItemInfo itemInfo)
        {
            if (itemInfo == null)
            {
                Debug.LogError("Cannot init item info as null!");
                return;
            }

            ItemInfo = itemInfo;
            _img.sprite = ItemInfo.ItemIcon;
        }
    }
}
