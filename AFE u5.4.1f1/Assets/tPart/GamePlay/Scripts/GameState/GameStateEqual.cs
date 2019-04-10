using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEqual : GameStateMethod
{
    [Header("Result Equal")]
    public bool resultEqual = false;

    [ContextMenu("OnClick")]
    public override void OnClick()
    {
        resultEqual = GameState.Instance.Equal(this.stateList);
    }
}
