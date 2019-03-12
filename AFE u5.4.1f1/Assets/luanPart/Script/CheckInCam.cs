using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// class gameobject must have Render Component

public class CheckInCam : MonoBehaviour
{

    ICharacterTranform thirdPersonUserControl;
    private void Awake()
    {
        thirdPersonUserControl = GetComponentInParent<ICharacterTranform>();
    }

    private void OnBecameVisible()
    {
        thirdPersonUserControl.InCamera = true;
        Debug.Log("OnBecame Visible");
    }

    private void OnBecameInvisible()
    {
        thirdPersonUserControl.InCamera = false;
        Debug.Log("OnBecame Invisible");
    }
}
