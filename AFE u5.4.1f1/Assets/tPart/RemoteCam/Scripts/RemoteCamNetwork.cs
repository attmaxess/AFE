using UnityEngine;
using System.Collections;

public class RemoteCamNetwork : Photon.MonoBehaviour
{
    private bool appliedInitialUpdate;

    void Awake()
    {

    }

    void Start()
    {
        //TODO: Bugfix to allow .isMine and .owner from AWAKE!        
        if (photonView.isMine)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }

        gameObject.name = gameObject.name + "_" + photonView.viewID;

#if UNITY_EDITOR
        gameObject.name += "_" + Application.dataPath;
#endif

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data            
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data            
            correctCamPos = (Vector3)stream.ReceiveNext();
            correctCamEuler = (Quaternion)stream.ReceiveNext();

            if (!appliedInitialUpdate)
            {
                appliedInitialUpdate = true;
                transform.position = correctCamPos;
                transform.rotation = correctCamEuler;
            }
        }
    }

    private Vector3 correctCamPos = Vector3.zero; //We lerp towards this
    private Quaternion correctCamEuler = Quaternion.identity; //We lerp towards this    

    void Update()
    {
        //Update remote player (smooth this, this looks good, at the cost of some accuracy)
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctCamPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctCamEuler, Time.deltaTime * 5);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.transform.position, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime * 5);
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }
}
