using Common.Dependencies;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.DebugTesting
{
    public class DepTesting : MonoBehaviour
    {
        void Start()
        {
            var equipHolder = DepResolver.Resolve<EquipmentHolder>();
            var commonGui = DepResolver.Resolve<CommonGui>();
            Assert.IsNotNull(equipHolder, "equipHolder != null");
            Assert.IsNotNull(commonGui, "commonGui != null");
        }
    }
   
}
