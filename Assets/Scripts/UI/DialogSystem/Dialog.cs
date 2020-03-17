using System;
using UnityEngine;

namespace GameUI
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] protected int _index;
        public int Index => _index;

        public virtual event Action OnDialogDone = () => { };
        
        public virtual void DialogDone()
        {
            Debug.Log($"Dialog {gameObject.name} , index: {_index} has done!");
            OnDialogDone();
        }
    }
}
