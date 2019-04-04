using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnSetSizePhotonGround : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Inputs")]
    public InputField ipf = null;

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

    [ContextMenu("OnClick")]
    public void OnClick()
    {        
        float size = float.Parse(ipf.text);
        if (isDebug) Debug.Log("Set Photon Plane size : " + size);
        currentPhotonGroundPlane.SetSize(size);
    }
}
