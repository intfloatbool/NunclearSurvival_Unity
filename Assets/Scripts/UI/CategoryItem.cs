using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class CategoryItem : MonoBehaviour
    {
        [SerializeField] private ItemType[] _categories;
        [SerializeField] private UiInventory _inventory;
        [SerializeField] private Button _btn;
        private void Awake()
        {
            Debug.Assert(_inventory != null, "_inventory != null");
            Debug.Assert(_btn != null, "_btn != null");
            
            _btn.onClick.AddListener(UpdateCategories);
        }

        public void UpdateCategories()
        {
            _inventory.ShowItemsByCategory(_categories);
        }
    }

}
