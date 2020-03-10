using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSceneBtn : GameButtonBase
{
    [SerializeField] private SceneType _sceneToGo;
    [SerializeField] private bool _isExternalDelay;
    [SerializeField] private float _externalDelay;
    protected override void OnClick()
    {
        if(_isExternalDelay)
        {
            SceneLoader.Instance.LoadScene(_sceneToGo);
        }
        else
        {
            SceneLoader.Instance.LoadScene(_sceneToGo, _externalDelay);
        }
        
    }
}
