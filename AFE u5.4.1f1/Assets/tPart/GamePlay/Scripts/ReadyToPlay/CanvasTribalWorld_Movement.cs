using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTribalWorld_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float distanceZ = 10;

    [Header("Input")]
    public MoveTo moveTo = null;
    public FaceTo faceTo = null;    

    public bool doneZoom = true;

    [ContextMenu("Zoom0")]
    public void Zoom0()
    {
        Zoom(Vector3.zero, Quaternion.identity);
    }

    [ContextMenu("ZoomCamAndDistance")]
    public void ZoomCamAndDistance()
    {
        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward.normalized * distanceZ;
        //Zoom(pos, cam.rotation);
        Zoom(pos, cam.eulerAngles.y, cam.eulerAngles.z);
    }

    public void Zoom(Vector3 pos, Quaternion qua)
    {
        StopAllC();
        StartCoroutine(C_Zoom(pos, qua));
    }

    IEnumerator C_Zoom(Vector3 pos, Quaternion qua)
    {
        doneZoom = false;

        moveTo.MoveToPos(pos);
        faceTo.ToQua(qua);

        yield return new WaitUntil(() => moveTo.doneMoveTo == true && faceTo.doneToQua == true);

        doneZoom = true;
        yield break;
    }

    public void Zoom(Vector3 pos, float toY, float toZ)
    {
        StopAllC();
        StartCoroutine(C_Zoom(pos, toY, toZ));
    }

    IEnumerator C_Zoom(Vector3 pos, float toY, float toZ)
    {
        doneZoom = false;

        moveTo.MoveToPos(pos);
        faceTo.ToEurY(toY);
        faceTo.ToEurZ(toZ);

        yield return new WaitUntil(() => moveTo.doneMoveTo == true && faceTo.doneToEurY == true && faceTo.doneToEurZ == true);

        doneZoom = true;
        yield break;
    }

    [ContextMenu("StopAllC")]
    public void StopAllC()
    {
        StopAllCoroutines();
    }   
}
