using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnGroundToLowestCenter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public FeaturesVisualizer fv = null;

    [Header("Output")]
    public Vector3 CurrentLowestPos = Vector3.zero;    

    PhotonGroundPlane _currentPhotonGroundPlane = null;
    PhotonGroundPlane currentPhotonGroundPlane
    {
        get
        {
            if (_currentPhotonGroundPlane == null)
                FindCurrentPhotonGroundPlane();
            return _currentPhotonGroundPlane;
        }
    }

    public void FindCurrentPhotonGroundPlane()
    {
        PhotonGroundPlane[] planes = FindObjectsOfType<PhotonGroundPlane>();
        for (int i = 0; i < planes.Length; i++)
        {
            PhotonView pv = planes[i].GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                _currentPhotonGroundPlane = planes[i];
                return;
            }
        }
        if (isDebug) Debug.Log("Can't find PhotonGroundPlane!!");
    }
        
    public void SnapToLowestCenter(Transform fv, Vector3[] points)
    {
        Mesh m = Application.isPlaying ? fv.GetComponent<MeshFilter>().mesh : fv.GetComponent<MeshFilter>().sharedMesh;
                
        if (points.Length == 0) return;
                
        Vector2 centerXZ = Vector2.zero;
        float lowestY = -Mathf.Infinity;

        for (int i = 0; i < points.Length; i++)
        {
            centerXZ += new Vector2(points[i].x, points[i].z);
            if (lowestY < points[i].y) lowestY = points[i].y;
        }
        centerXZ /= points.Length;

        currentPhotonGroundPlane.transform.position = fv.TransformPoint(new Vector3(centerXZ.x, lowestY, centerXZ.y));
        CurrentLowestPos = currentPhotonGroundPlane.transform.position;
    }

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnGroundToLowestCenter SnapToLowestCenter");

        if (fv == null) return;

        if (fv.delAfter_SnapGroud == null)
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud == null");
            fv.delAfter_SnapGroud += SnapToLowestCenter;
        }
        else
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud != null");
            fv.delAfter_SnapGroud = null;
            CurrentLowestPos = Vector3.zero;
        }

        Invoke("DebugCurrentLowestPos", 1f);
    }

    [ContextMenu("DebugCurrentLowestPos")]
    public void DebugCurrentLowestPos()
    {
        Debug.Log("CurrentLowestPos : " + CurrentLowestPos);
    }

    [ContextMenu("DebugCurrentPhotonPos")]
    public void DebugCurrentPhotonPos()
    {
        Debug.Log("DebugCurrentPhotonPos : " + currentPhotonGroundPlane.transform.position);
    }
}