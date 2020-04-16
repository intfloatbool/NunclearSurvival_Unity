using UnityEngine;

namespace GameUI
{
    public interface IItemHandler
    {
        Transform ItemsParent { get; }
        void MakeActionWithItem(InventoryItemUi itemUi);
        void OnItemDrag(InventoryItemUi itemUi);
        void OnItemUp(InventoryItemUi itemUi);
        void OnItemDropAtHandler(InventoryItemUi itemUi);
    }
}

