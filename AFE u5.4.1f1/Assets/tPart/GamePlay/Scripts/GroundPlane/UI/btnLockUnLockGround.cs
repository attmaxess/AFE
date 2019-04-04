using AFE.BaseGround;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnLockUnLockGround : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    [Header("Input")]
    public AFEBaseMarkerManager baseManager = null;
    public Text txt = null;

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
        baseManager.useUpdate = !baseManager.useUpdate;
        txt.text = baseManager.useUpdate == true ? "Lock Ground" : "UnLock Ground";
    }

    [ContextMenu("DebugCurrentPhotonPos")]
    public void DebugCurrentPhotonPos()
    {
        Debug.Log("DebugCurrentPhotonPos : " + currentPhotonGroundPlane.transform.position);
    }
}