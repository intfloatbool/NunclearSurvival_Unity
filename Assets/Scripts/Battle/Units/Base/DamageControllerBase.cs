using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public abstract class DamageControllerBase : MonoBehaviour
    {
        [SerializeField] protected UnitDamage _unitDamage;

        protected virtual void Awake()
        {
            Assert.IsNotNull(_unitDamage, "_unitDamage != null");
        }
    }
}
