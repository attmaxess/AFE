using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBeetsoftLogo : MonoBehaviour
{
    public string logo = string.Empty;
    public Transform parent = null;
    public Vector3 localPos = Vector3.zero;
    public Quaternion localRot = Quaternion.identity;
    public Vector3 localSca = Vector3.zero;

    [ContextMenu("NetInstance")]
    public void NetInstance()
    {
        GameObject newLogo = PhotonNetwork.Instantiate(logo, Vector3.zero, Quaternion.identity, 0);
        newLogo.transform.parent = parent;
        newLogo.transform.localPosition = localPos;
        newLogo.transform.localRotation = localRot;
        newLogo.transform.localScale = localSca;
    }

    [Header("Process")]
    public bool InstanceAtStart = true;

    private void Start()
    {
        if (InstanceAtStart) NetInstance();
    }    
}
