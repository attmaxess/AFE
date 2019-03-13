using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScanValues
{
    public Vector3 position;
    public Vector3 localScale;
    public Quaternion rotation;
    public Transform transform;

    public static ScanValues GetData(GameObject obj)
    {
        return GetData(obj.transform);
    }

    public static ScanValues GetData(Transform trans)
    {
        ScanValues vl = new ScanValues();
        vl.position = trans.transform.position;
        vl.localScale = trans.transform.localScale;
        vl.rotation = trans.transform.rotation;
        vl.transform = trans;
        return vl;
    }

    public static ScanValues GetData(Vector3 pos, Quaternion rot)
    {
        ScanValues vl = new ScanValues();
        vl.position = pos;
        vl.rotation = rot;
        return vl;
    }
}
