using UnityEngine;

namespace Items
{
    public class ItemCraftSystem : UnitySingletonBase<ItemCraftSystem>
    {
        protected override ItemCraftSystem GetInstance() => this;

        public ItemName? TryCraftItem(params ItemName[] names)
        {
            //TODO: Realize craft logic
            return null;
        }
        
    }
}
