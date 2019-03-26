using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnGroundToBestLowestPoint : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public FeaturesVisualizer fv = null;

    [Header("Output")]
    public int CurrentBestIndex = -1;
    public Vector3 CurrentBestPos = -Vector3.one;

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

    public void SnapToBestLowestPoint(Transform fv, Vector3[] points)
    {
        if (points.Length == 0) return;
        if (CurrentBestPos != -Vector3.one && CurrentBestIndex != -1)
        {
            currentPhotonGroundPlane.transform.position = CurrentBestPos;
        }
        else
        {
            Vector2 centerXZ = Vector2.zero;
            float HighestY = -Mathf.Infinity;

            CurrentBestIndex = -1;
            for (int i = 0; i < points.Length; i++)
            {
                centerXZ += new Vector2(points[i].x, -points[i].z);
                if (HighestY < points[i].y)
                {
                    HighestY = points[i].y;
                    CurrentBestIndex = i;
                }
            }
            centerXZ /= points.Length;

            currentPhotonGroundPlane.transform.position = fv.TransformPoint(new Vector3(centerXZ.x, HighestY, centerXZ.y));
            CurrentBestPos = currentPhotonGroundPlane.transform.position;
        }
    }

    [ContextMenu("OnClick")]
    public void OnClick()
    {
        if (isDebug) Debug.Log("btnGroundToHighestCenter SnapToHighestCenter");

        if (fv == null) return;

        if (fv.delAfter_SnapGroud == null)
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud == null");
            CurrentBestPos = -Vector3.one;
            CurrentBestIndex = -1;
            fv.delAfter_SnapGroud += SnapToBestLowestPoint;
        }
        else
        {
            if (isDebug) Debug.Log("fv.delAfter_SnapGroud != null");
            fv.delAfter_SnapGroud = null;
            CurrentBestPos = -Vector3.one;
            CurrentBestIndex = -1;
        }

        Invoke("DebugCurrentHighestPos", 1f);
    }

    [ContextMenu("DebugCurrentHighestPos")]
    public void DebugCurrentHighestPos()
    {
        Debug.Log("CurrentHighestPos : " + CurrentBestPos);
    }

    [ContextMenu("DebugCurrentPhotonPos")]
    public void DebugCurrentPhotonPos()
    {
        Debug.Log("DebugCurrentPhotonPos : " + currentPhotonGroundPlane.transform.position);
    }
}