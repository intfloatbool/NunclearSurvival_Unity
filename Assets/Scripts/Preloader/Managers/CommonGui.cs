using Common.Dependencies;
using GameUI;
using UnityEngine;

namespace SingletonsPreloaders
{
    public class CommonGui : UnitySingletonBase<CommonGui>, ISingletonDependency
    {
        [SerializeField] private CustomDialog _customDialog;
        protected override CommonGui GetInstance() => this;

        public CustomDialog GetDialog()
        {
            return _customDialog;
        }

        public void SelfRegister()
        {
            DepResolver.RegisterDependency(this);
        }
    }

}

