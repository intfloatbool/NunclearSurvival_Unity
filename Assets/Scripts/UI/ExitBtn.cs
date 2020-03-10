using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtn : GameButtonBase
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}
