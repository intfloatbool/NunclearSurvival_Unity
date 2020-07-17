using System.Collections;
using System.Collections.Generic;
using GameUI;
using UnityEngine;

namespace NunclearGame.BonfireSceneUI
{
    public class CraftPanel : MonoBehaviour, IItemInteractorUI
    {
        public void OnItemDroppedHere(InventoryItemUi itemUi)
        {
            //TEST
            Debug.Log($"Item {itemUi.CurrentItemInfo.ItemName} dropped in craft panel!");
        }
    }
}
