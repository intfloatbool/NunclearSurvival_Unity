using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.Battle
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _fireTransformPoint;
        public Transform FireTransformPoint => _fireTransformPoint;

        private void Awake()
        {
            Assert.IsNotNull(_fireTransformPoint, "_fireTransformPoint != null");
        }
    }
}

