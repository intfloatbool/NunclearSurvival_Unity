using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneBtn : GameButtonBase
{
    [SerializeField] private SceneType _sceneToGo;
    protected override void OnClick()
    {
        SceneLoader.Instance.LoadScene(_sceneToGo);
    }
}
