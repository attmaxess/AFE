using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSnapCharacterPlaneJoyStick : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Snap")]
    public float offsetY = 0.5f;

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnSnapCharacterPlaneJoyStick OnClick!!");

        PlaneJoystick pj = FindObjectOfType<PlaneJoystick>();
        if (pj == null)
        {
            Debug.Log("Can't find PlaneJoystick");
            return;
        }

        //Rigidbody rb = pj.joystickCharacter.transform.GetComponent<Rigidbody>();

        //bool lastKinematic = rb.isKinematic;
        //bool lastUseGravity = rb.useGravity;
        //rb.isKinematic = true;
        //rb.useGravity = false;

        //rb.velocity = Vector3.zero;        

        //pj.rotateChar.transform.position = pj.transform.position + new Vector3(0, offsetY, 0);

        //rb.isKinematic = lastKinematic;
        //rb.useGravity = lastUseGravity;
    }
}
