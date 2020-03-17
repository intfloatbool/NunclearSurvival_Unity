using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class DialogController : MonoBehaviour
    {
        [SerializeField] protected List<Dialog> _dialogs;
        [SerializeField] protected int _currentDialogIndex = -1;
        protected virtual void Start()
        {
            InitDialogs();
        }

        protected virtual void InitDialogs()
        {
            foreach (var dialog in _dialogs)
            {
                dialog.OnDialogDone += ShowNextDialog;
            }
        }

        protected virtual void ShowNextDialog()
        {
            ShowDialogByIndex(_currentDialogIndex + 1);
        }

        public virtual void ShowDialogByIndex(int index)
        {
            foreach (var dialog in _dialogs)
            {
                dialog.gameObject.SetActive(dialog.Index == index);
            }

            _currentDialogIndex = index;
        }

        public virtual void HideAllDialogs()
        {
            _dialogs.ForEach(d => d.gameObject.SetActive(false));
        }
    }
}

