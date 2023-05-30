using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameUI;
using NunclearGame.Static;
using Player;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.BonfireSceneUI
{
    public class UpdateItemsDialogController : MonoBehaviour
    {
        private PlayerInventory _playerInventory;
        private CustomDialog _dialog;
        private Stack<InventoryItem> _itemStack = new Stack<InventoryItem>();
        private InventoryItem _lastItem;
        private bool _isShowNext;

        private Coroutine _showingCoroutine;
        
        private void Awake()
        {
            _playerInventory = GlobalPlayer.Instance?.PlayerInventory;
            Assert.IsNotNull(_playerInventory, "_playerInventory != null");
            if (_playerInventory != null)
            {
                _playerInventory.OnItemAddedRef += AddItemDialogInStack;
            }

            _dialog = CommonGui.Instance?.GetDialog();
            Assert.IsNotNull(_dialog, "_dialog != null");
        }

        private void OnDestroy()
        {
            if (_playerInventory != null)
            {
                _playerInventory.OnItemAddedRef -= AddItemDialogInStack;
            }
        }

        private void AddItemDialogInStack(InventoryItem inventoryItem)
        {
            if (inventoryItem == null)
            {
                Debug.LogError("Cannot handle null inventoryItem!");
                return;
            }
            _itemStack.Push(inventoryItem.Clone());

            ShowDialogs();
        }

        private void ShowDialogs()
        {
            if (_showingCoroutine != null)
            {
                return;
            }

            _showingCoroutine = StartCoroutine(ShowDialogsCoroutine());
        }

        private IEnumerator ShowDialogsCoroutine()
        {
            while (_itemStack.Count > 0)
            {
                var item = _itemStack.Pop();
                if(item == null)
                    continue;
                ShowDialogAboutItem(item);
                while (!_isShowNext)
                {
                    yield return null;
                }
                yield return null;
            }
            yield return null;
            ClearDialogs();
            _showingCoroutine = null;
        }

        private void ClearDialogs()
        {
            _itemStack.Clear();
        }

        private void ShowDialogAboutItem(InventoryItem inventoryItem)
        {
            _isShowNext = false;
            _dialog
                .ResetDialog()
                .SetHeader(GameHelper.LocKeys.NEW_ITEM_DIALOG_HEADER_KEY, inventoryItem.ItemInfo.ItemIcon)
                .SetDialogDescription(inventoryItem.ItemInfo.ItemViewNameKey)
                .AddButton(GameHelper.LocKeys.OKAY_LABEL_KEY, OnDialogBtnClick)
                .ShowDialog();

        }

        private void OnDialogBtnClick()
        {
            _isShowNext = true;
        }
        
        
    }
}
