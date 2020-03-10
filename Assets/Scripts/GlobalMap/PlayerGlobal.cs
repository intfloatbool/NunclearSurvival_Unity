using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _movingKey = "isWalk";
    [SerializeField] private MoveAvatar _goMoveAvatar;

    private void Awake()
    {
        _goMoveAvatar.OnMove += (isMoving) =>
        {
            _animator.SetBool(_movingKey, isMoving);
        };
    }
}
