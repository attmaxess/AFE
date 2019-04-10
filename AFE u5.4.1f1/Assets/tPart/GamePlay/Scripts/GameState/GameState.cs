using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState>
{
    [Serializable]
    public enum eGameState
    {
        ReadyToPlay,
        ArKit,
        PlaceNote
    }

    public List<eGameState> stateList = new List<eGameState>();

    public void AddOnce(List<eGameState> states)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (stateList.FindIndex((x) => x == states[i]) == -1)
            {
                stateList.Add(states[i]);
            }
        }
    }

    public void Remove(List<eGameState> states)
    {
        for (int i = 0; i < states.Count; i++)
        {
            stateList.Remove(states[i]);
        }
    }

    public void BeThis(List<eGameState> states)
    {
        this.stateList = states;
    }

    public bool Equal(List<eGameState> states)
    {
        if (this.stateList.Count != states.Count) return false;
        foreach (eGameState e in this.stateList)
            if (states.FindIndex((x) => x == e) == -1) return false;
        return true;            
    }
}
