using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnUnlockCharacter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnUnlockCharacter OnClick!!");

        PlaneJoystick pj = FindObjectOfType<PlaneJoystick>();
        if (pj == null)
        {
            Debug.Log("Can't find PlaneJoystick");
            return;
        }

        pj.useUpdate = true;
    }
}
