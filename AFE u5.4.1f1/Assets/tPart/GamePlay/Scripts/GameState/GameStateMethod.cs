using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMethod : MonoBehaviour, GameStateIMethod
{
    public List<GameState.eGameState> stateList = new List<GameState.eGameState>();

    public virtual void OnClick()
    {        
    }
}
