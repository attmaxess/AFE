using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// class gameobject must have Render Component

public class CheckInCam : MonoBehaviour
{

    ThirdPersonUserControl thirdPersonUserControl;
    private void Awake()
    {
        thirdPersonUserControl = GetComponentInParent<ThirdPersonUserControl>();
    }

    private void OnBecameVisible()
    {
        Debug.Log("OnBecame Visible");
        thirdPersonUserControl.InScreen = true;
    }

    private void OnBecameInvisible()
    {
        Debug.Log("OnBecame Invisible");
        thirdPersonUserControl.InScreen = false;
    }
}
