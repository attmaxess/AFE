using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSnapCharacterToCenterOfGround : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnSnapCharacterToCenterOfGround OnClick!!");

        PlaneJoystick pj = FindObjectOfType<PlaneJoystick>();
        if (pj == null)
        {
            Debug.Log("Can't find PlaneJoystick");
            return;
        }

        PhotonGroundPlane ground = FindObjectOfType<PhotonGroundPlane>();
        if (ground == null)
        {
            Debug.Log("Can't find PhotonGroundPlane");
            return;
        }

        pj.useUpdate = false;
        pj.transform.position = ground.transform.position;
    }
}
