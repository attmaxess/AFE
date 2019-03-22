using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnGroundToHighestCenter : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public FeaturesVisualizer fv = null;

    [Header("Output")]
    public Vector3 CurrentHighestPos = Vector3.zero;    

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
        
    public void SnapToHighestCenter(Transform fv, Vector3[] points)
    {
        Mesh m = Application.isPlaying ? fv.GetComponent<MeshFilter>().mesh : fv.GetComponent<MeshFilter>().sharedMesh;
                
        if (points.Length == 0) return;
                
        Vector2 centerXZ = Vector2.zero;
        float HighestY = Mathf.Infinity;

        for (int i = 0; i < points.Length; i++)
        {
            centerXZ += new Vector2(points[i].x, points[i].z);
            if (HighestY > points[i].y) HighestY = points[i].y;
        }
        centerXZ /= points.Length;

        currentPhotonGroundPlane.transform.position = fv.TransformPoint(new Vector3(centerXZ.x, HighestY, centerXZ.y));
        CurrentHighestPos = currentPhotonGroundPlane.transform.position;
    }

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnGroundToHighestCenter SnapToHighestCenter");

        if (fv == null) return;

        if (fv.delAfter_SnapGroud == null)
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud == null");
            fv.delAfter_SnapGroud += SnapToHighestCenter;
        }
        else
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud != null");
            fv.delAfter_SnapGroud = null;
            CurrentHighestPos = Vector3.zero;
        }

        Invoke("DebugCurrentHighestPos", 1f);
    }

    [ContextMenu("DebugCurrentHighestPos")]
    public void DebugCurrentHighestPos()
    {
        Debug.Log("CurrentHighestPos : " + CurrentHighestPos);
    }

    [ContextMenu("DebugCurrentPhotonPos")]
    public void DebugCurrentPhotonPos()
    {
        Debug.Log("DebugCurrentPhotonPos : " + currentPhotonGroundPlane.transform.position);
    }
}