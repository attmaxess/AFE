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
            // cameraScript.enabled = false;
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
            // stream.SendNext((int)controllerScript._characterState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
          //  stream.SendNext(transform.localScale);
            stream.SendNext(GetComponent<Rigidbody>().velocity);

            // input
           /* stream.SendNext(controllerScript.attack);
            stream.SendNext(controllerScript.skill_1);
            stream.SendNext(controllerScript.skill_2);
            stream.SendNext(controllerScript.skill_3);
            stream.SendNext(controllerScript.skill_4);
            stream.SendNext(controllerScript.run);
            stream.SendNext(controllerScript.idle);
            stream.SendNext(controllerScript.hit);   */

        }
        else
        {
            //Network player, receive data
            //controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
           // correctPlayerScale = (Vector3)stream.ReceiveNext();
            GetComponent<Rigidbody>().velocity = (Vector3)stream.ReceiveNext();

         /*   controllerScript.attack = (bool)stream.ReceiveNext();
            controllerScript.skill_1 = (bool)stream.ReceiveNext();
            controllerScript.skill_2 = (bool)stream.ReceiveNext();
            controllerScript.skill_3 = (bool)stream.ReceiveNext();
            controllerScript.skill_4 = (bool)stream.ReceiveNext();
            controllerScript.run = (bool)stream.ReceiveNext();
            controllerScript.idle = (bool)stream.ReceiveNext();
            controllerScript.hit = (bool)stream.ReceiveNext();  */


            if (!appliedInitialUpdate)
            {
                appliedInitialUpdate = true;
                transform.position = correctPlayerPos;
                transform.rotation = correctPlayerRot;
              //  transform.localScale = correctPlayerScale;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
    private Vector3 correctPlayerScale = Vector3.zero; //We lerp towards this

    void Update()
    {
        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
       //     transform.localScale = Vector3.Lerp(transform.localScale, correctPlayerScale, Time.deltaTime * 5);
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //We know there should be instantiation data..get our bools from this PhotonView!
        object[] objs = photonView.instantiationData; //The instantiate data..
       // bool[] mybools = (bool[])objs[0];   //Our bools!

        //disable the axe and shield meshrenderers based on the instantiate data
        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
        //rens[0].enabled = mybools[0];//Axe
       // rens[1].enabled = mybools[1];//Shield

    }

}