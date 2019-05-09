using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFEInspector : MonoBehaviour
{
    //-----------------------------------------------------------------------------------------------------------------    
    public GameMode gameMode = GameMode.None;

    [ContextMenu("DoUpdateFromUFE")]
    public void DoUpdateFromUFE()
    {
        gameMode = UFE.gameMode;
    }
}
