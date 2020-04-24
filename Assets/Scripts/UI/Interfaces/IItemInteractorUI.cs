using System;
using UnityEngine;

namespace GameUI
{
    public interface IItemInteractorUI
    {
        void OnItemDroppedHere(InventoryItemUi itemUi);     
    }

}