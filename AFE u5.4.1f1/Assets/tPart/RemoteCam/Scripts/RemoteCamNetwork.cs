using UnityEngine;
using System.Collections;

public class RemoteCamNetwork : Photon.MonoBehaviour
{
    [Header("Current Cam")]
    public Transform SimulCam = null;

    private bool appliedInitialUpdate;

    void Awake()
    {

    }

    void Start()
    {
        //TODO: Bugfix to allow .isMine and .owner from AWAKE!        
        if (photonView.isMine)
        {
            if (SimulCam != null)
            {
                SimulCam.transform.position = Camera.main.transform.position;
                SimulCam.transform.rotation = Camera.main.transform.rotation;
            }
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
            stream.SendNext(SimulCam.transform.position);
            stream.SendNext(SimulCam.transform.rotation);
        }
        else
        {
            //Network player, receive data            
            correctCamPos = (Vector3)stream.ReceiveNext();
            correctCamEuler = (Quaternion)stream.ReceiveNext();

            if (!appliedInitialUpdate)
            {
                appliedInitialUpdate = true;
                SimulCam.position = correctCamPos;
                SimulCam.rotation = correctCamEuler;
            }
        }
    }

    private Vector3 correctCamPos = Vector3.zero; //We lerp towards this
    private Quaternion correctCamEuler = Quaternion.identity; //We lerp towards this    

    void Update()
    {
        if (SimulCam == null) return;

        //Update remote player (smooth this, this looks good, at the cost of some accuracy)
        if (!photonView.isMine)
        {
            SimulCam.position = Vector3.Lerp(SimulCam.position, correctCamPos, Time.deltaTime * 5);
            SimulCam.rotation = Quaternion.Lerp(SimulCam.rotation, correctCamEuler, Time.deltaTime * 5);
        }
        else
        {
            SimulCam.position = Vector3.Lerp(SimulCam.position, Camera.main.transform.position, Time.deltaTime * 5);
            SimulCam.rotation = Quaternion.Lerp(SimulCam.rotation, Camera.main.transform.rotation, Time.deltaTime * 5);
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }
}
