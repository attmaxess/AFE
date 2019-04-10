using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBeThis : GameStateMethod
{
    [ContextMenu("OnClick")]
    public override void OnClick()
    {
        GameState.Instance.BeThis(this.stateList);
    }
}
