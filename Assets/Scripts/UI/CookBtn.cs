using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class CookBtn : MonoBehaviour
    {
        [SerializeField] private BonfireCookerUI _bonfireCooker;
        [SerializeField] private Button _btn;

        private void Start()
        {
            Debug.Assert(_bonfireCooker != null, "_bonfireCooker != null");
            Debug.Assert(_btn != null, "_btn != null");

            if (_bonfireCooker != null)
            {
                _bonfireCooker.OnItemAdd += OnItemAdded;
            }

            if(_btn != null)
            {
                _btn.interactable = false;
            }
        }

        private void OnItemAdded(InventoryItemUi itemUi)
        {
            var recipeItemInfo = _bonfireCooker?.RecipeItem?.LastItemInfo;
            var cookItemInfo = _bonfireCooker?.CookItem?.LastItemInfo;
            var isActiveButton = false;

            //TODO: Check recieps to active button!

            if(recipeItemInfo != null && cookItemInfo != null)
            {

            }

            _btn.interactable = isActiveButton;
        }
    }
}

