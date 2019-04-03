using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnCloudPointSnapGround : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = true;

    public Text text = null;
    public Vector3 currentCordinate = -Vector3.one;

    public void SetCurrentCordinate(int index, Vector3 cor)
    {
        currentCordinate = cor;
        text.text = index.ToString() + ". " + "(" + cor.x + ", " + cor.y + ", " + cor.z + ")";
    }

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
        if (isDebug) Debug.Log("OnClick at cordinate : " + currentCordinate);
        currentPhotonGroundPlane.transform.position = currentCordinate;
    }
}
