using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class SwitchMenuItems : MonoBehaviour
    {
        [SerializeField] private List<MenuItem> _menuItems;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _hidedColor;
        [SerializeField] private MenuItem _selectedItem;

        private void Start()
        {
            InitMenuItems();

            if(_selectedItem != null)
            {
                _selectedItem.ImitateClick();
            }
        }

        private void InitMenuItems()
        {
            foreach(var menuItem in _menuItems)
            {
                menuItem.OnClickItem += OnItemSwitched;
            }
        }

        private void HighlightItem(MenuItem item)
        {
            item.SetColor(_activeColor);
        }

        private void HideItem(MenuItem item)
        {
            item.SetColor(_hidedColor);
        }

        private void OnItemSwitched(MenuItem item)
        {
            HideAllButOneHighlight(item);
        }

        private void HideAllButOneHighlight(MenuItem item)
        {
            _menuItems.ForEach(mi =>
            {
                if(mi == item)
                {
                    HighlightItem(mi);
                }
                else
                {
                    HideItem(mi);
                }
            });
        }

    }
}

