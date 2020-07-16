using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField] private Transform _followTransform;

        public Transform FollowTransform
        {
            get { return _followTransform; }
            set { this._followTransform = value; }
        }
        
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _rotSpeed = 6f;

        private void Update()
        {
            if (_followTransform == null)
                return;

            transform.position =
                Vector3.Lerp(transform.position, _followTransform.position, _moveSpeed * Time.deltaTime);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, _followTransform.rotation, _rotSpeed * Time.deltaTime);
        }
    }
}
