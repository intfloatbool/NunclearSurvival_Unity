using GameUI;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class CommonGui : UnitySingletonBase<CommonGui>
    {
        [SerializeField] private CustomDialog _customDialog;
        protected override CommonGui GetInstance() => this;

        public CustomDialog GetDialog()
        {
            return _customDialog;
        }
    }

}

