using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattlePlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private Joystick _joystick;

        private void Update()
        {
            var horizontal = _joystick.Horizontal;
            var vertical = _joystick.Vertical;
            var rotateVector = new Vector3(-vertical, horizontal);
            transform.Rotate(rotateVector * _rotationSpeed * Time.deltaTime, Space.World);
            transform.eulerAngles = 
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                0);
        }
    }
}

