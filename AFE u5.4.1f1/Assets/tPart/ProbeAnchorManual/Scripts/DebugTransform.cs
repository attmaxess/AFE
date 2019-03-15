using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTransform : MonoBehaviour
{
    [ContextMenu("DebugOut")]
    public void DebugOut()
    {
        Debug.Log("Pos : " + StrV3(transform.position));
        Debug.Log("Rot : " + transform.rotation);
        Debug.Log("Eul : " + StrV3(transform.rotation.eulerAngles));
        Debug.Log("Sca : " + StrV3(transform.lossyScale));

        Debug.Log("lPos : " + StrV3(transform.localPosition));
        Debug.Log("lRot : " + transform.localRotation);
        Debug.Log("lEul : " + StrV3(transform.localRotation.eulerAngles));
        Debug.Log("lSca : " + StrV3(transform.localScale));
    }

    string StrV3(Vector3 v)
    {
        return "(" + v.x + ", " + v.y + ", " + v.z + ")";
    }
}
