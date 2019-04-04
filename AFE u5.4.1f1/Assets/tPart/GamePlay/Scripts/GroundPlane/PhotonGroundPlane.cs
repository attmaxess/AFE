using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class PhotonGroundPlane : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool appliedInitialUpdate;

    void Awake()
    {

    }

    void Start()
    {
        //TODO: Bugfix to allow .isMine and .owner from AWAKE!        
        if (photonView.IsMine)
        {

        }

        gameObject.name = gameObject.name + "_" + photonView.ViewID;

#if UNITY_EDITOR
        gameObject.name += "_" + Application.dataPath;
#endif

    }

    void Update()
    {

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

    [Header("Setting Transform")]
    public MeshCollider mc = null;
    public MeshRenderer mr = null;

    public void SetSize(float size)
    {
        mr.transform.localScale = new Vector3(size, size, size);
    }

    public void MoveAStep(float step)
    {
        transform.position += new Vector3(0, step, 0);
    }
}
