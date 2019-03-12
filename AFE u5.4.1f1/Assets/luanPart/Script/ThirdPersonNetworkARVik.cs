using UnityEngine;

public class ThirdPersonNetworkARVik : Photon.MonoBehaviour
{
    ThirdPersonCameraNET cameraScript;
    ThirdPersonControllerNET controllerScript;
    private bool appliedInitialUpdate;

    void Awake()
    {
        cameraScript = GetComponent<ThirdPersonCameraNET>();
        controllerScript = GetComponent<ThirdPersonControllerNET>();
    }

    void Start()
    {
        //TODO: Bugfix to allow .isMine and .owner from AWAKE!
        if (photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts            
            controllerScript.enabled = true;
        }
        else
        {
            controllerScript.enabled = true;
        }

        controllerScript.SetIsRemotePlayer(!photonView.isMine);

        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data            
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);            
            stream.SendNext(GetComponent<Rigidbody>().velocity);
        }
        else
        {
            //Network player, receive data            
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();            
            GetComponent<Rigidbody>().velocity = (Vector3)stream.ReceiveNext();

            if (!appliedInitialUpdate)
            {
                appliedInitialUpdate = true;
                transform.position = correctPlayerPos;
                transform.rotation = correctPlayerRot;                
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this    

    void Update()
    {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);                        
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        
    }
}