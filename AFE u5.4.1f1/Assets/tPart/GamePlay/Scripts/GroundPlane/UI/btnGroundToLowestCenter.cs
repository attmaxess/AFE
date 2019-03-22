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

    [ContextMenu("SnapToLowestCenter")]
    public void SnapToLowestCenter()
    {
        if (isDebug) Debug.Log("SnapToLowestCenter");

        if (LibPlacenote.Instance.GetStatus() != LibPlacenote.MappingStatus.RUNNING)
        {
            return;
        }
        
        Mesh m = Application.isPlaying ? fv.GetComponent<MeshFilter>().mesh : fv.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vert = m.vertices;
        if (vert.Length == 0) return;
                
        Vector2 centerXZ = Vector2.zero;
        float lowestY = -Mathf.Infinity;

        for (int i = 0; i < vert.Length; i++)
        {
            centerXZ += new Vector2(vert[i].x, vert[i].z);
            if (lowestY < vert[i].y) lowestY = vert[i].y;
        }
        centerXZ /= vert.Length;

        currentPhotonGroundPlane.transform.position = fv.transform.TransformPoint(new Vector3(centerXZ.x, lowestY, centerXZ.y));
        if (isDebug) Debug.Log("Lowest center " + currentPhotonGroundPlane.transform.position);
    }
}