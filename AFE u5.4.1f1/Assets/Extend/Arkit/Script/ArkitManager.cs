using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
/// <summary>
/// Arkit manager. Use for convert acoordinate from vuforia to arkit
/// </summary>
public class ArkitManager : MonoBehaviour, IArkitManager
{
    private bool isScan = true;
    private Action<ScanValues> saveCallbackFoundPlane;
    private UnityARAnchorManager unityARAnchorManager;

    public virtual void StartScanPlane(Action<ScanValues> callbackOnFoundPlane)
    {
        saveCallbackFoundPlane = callbackOnFoundPlane;
        isScan = true;
    }
    public virtual void StopScanPlane()
    {
        isScan = false;
    }
    public virtual void Start()
    {
        unityARAnchorManager = new UnityARAnchorManager();
    }
    void Update()
    {
        //Debug.Log("Scan -- " + unityARAnchorManager + "  isScan = " + isScan);
#if UNITY_EDITOR
        return;
#endif
        if (unityARAnchorManager == null)
        {
            return;
        }
        if (isScan)
        {
            List<ARPlaneAnchorGameObject> arpags = new List<ARPlaneAnchorGameObject>();
            arpags.AddRange(unityARAnchorManager.GetCurrentPlaneAnchors());
            if (arpags.Count >= 1)
            {
                GameObject plane = arpags[0].gameObject;
                OnFoundFloor(plane);
            }
        }
    }
    void OnFoundFloor(GameObject floor)
    {
        if (saveCallbackFoundPlane != null)
        {
          //  Debug.Log("Found Floor !!  " + floor);
            ScanValues scan = ScanValues.GetData(floor);
            saveCallbackFoundPlane(scan);
        }
    }
    void OnDestroy()
    {
        if (unityARAnchorManager != null)
        {
            unityARAnchorManager.Destroy();
        }
    }
}

/*
Save data one item at Vuforia
 */
public struct OneDataCharacter
{
    public Vector3 Position;
    public Vector3 eulesAngle;
}

public class SaveDataVuforia
{
    public static List<OneDataCharacter> ListOneData = new List<OneDataCharacter>();
    public static OneDataCharacter SaveDataImageTarget;
}