using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Photon.Pun;

public delegate void JumpDelegate();

public class ThirdPersonControllerNET : MonoBehaviourPunCallbacks, ICharacterTranform, IPunObservable
{
    public Rigidbody target;
    // The object we're steering
    public float speed = 1.0f, walkSpeedDownscale = 2.0f, turnSpeed = 2.0f, mouseTurnSpeed = 0.3f, jumpSpeed = 1.0f;
    // Tweak to ajust character responsiveness
    public LayerMask groundLayers = -1;
    // Which layers should be walkable?
    // NOTICE: Make sure that the target collider is not in any of these layers!
    public float groundedCheckOffset = 0.7f;
    // Tweak so check starts from just within target footing
    public bool
        showGizmos = true,
        // Turn this off to reduce gizmo clutter if needed
        requireLock = true,
        // Turn this off if the camera should be controllable even without cursor lock
        controlLock = false;
    // Turn this on if you want mouse lock controlled by this script
    public JumpDelegate onJump = null;
    // Assign to this delegate to respond to the controller jumping

    private const float inputThreshold = 0.01f,
        groundDrag = 5.0f,
        directionalJumpFactor = 0.7f;
    // Tweak these to adjust behaviour relative to speed
    private const float groundedDistance = 0.5f;
    // Tweak if character lands too soon or gets stuck "in air" often

    public bool grounded, walking;

    #region player state 
    protected BaseCharaterState currentStatePlayer;
    Dictionary<StateCharacter, BaseCharaterState> listState = new Dictionary<StateCharacter, BaseCharaterState>();
    [SerializeField]
    StateCharacter _currentStateType;

    #endregion

    private bool isRemotePlayer = true;

    public event Action OnAttack;
    public event Action OnSkill1;
    public event Action OnSkill2;
    public event Action OnSkill3;
    public event Action OnSkill4;
    public event Action OnIdle;

    public bool Grounded
    // Make our grounded status available for other components
    {
        get
        {
            return grounded;
        }
    }

    public bool InCamera
    {
        get;
        set;
    }

    public void SetIsRemotePlayer(bool val)
    {
        Debug.Log("SetIsRemotePlayer " + val);
        isRemotePlayer = val;
    }

    void Reset()
    // Run setup on component attach, so it is visually more clear which references are used
    {
        Setup();
    }


    void Setup()
    // If target is not set, try using fallbacks
    {
        if (target == null)
        {
            target = GetComponent<Rigidbody>();
        }
    }

    void Start()
    // Verify setup, configure rigidbody
    {
        Setup();
        // Retry setup if references were cleared post-add

        if (target == null)
        {
            Debug.LogError("No target assigned. Please correct and restart.");
            enabled = false;
            return;
        }

        target.freezeRotation = true;
        // We will be controlling the rotation of the target, so we tell the physics system to leave it be
        walking = false;

        // resigter Event from ui controller
        if (GetComponent<PhotonView>().IsMine)
        {
            GameManagerArVik.Singleton.attack += Singleton_Attack;
            GameManagerArVik.Singleton.skill1 += Singleton_skill1;
            GameManagerArVik.Singleton.skill2 += Singleton_skill2;
            GameManagerArVik.Singleton.skill3 += Singleton_skill3;
            GameManagerArVik.Singleton.skill4 += Singleton_skill4;
        }

        #region initialize player state
        /*  var states = GetComponentsInChildren<BaseCharaterState>();
          foreach (var item in states)
          {
              listState.Add(item.stateType, item);
              item.Player = this;
          }     */
        #endregion
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
    private Vector3 correctPlayerScale = Vector3.zero; //We lerp towards this
    private bool appliedInitialUpdate;

    bool skill_1;
    public bool skill_1Rpc
    {
        get { return skill_1; }
        set
        {
            skill_1 = value;
            photonView.RPC("RpcSkill_1", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool skill_2;
    public bool skill_2Rpc
    {
        get { return skill_2; }
        set
        {
            skill_2 = value;
            photonView.RPC("RpcSkill_2", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool skill_3;
    public bool skill_3Rpc
    {
        get { return skill_3; }
        set
        {
            skill_3 = value;
            photonView.RPC("RpcSkill_3", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool skill_4;
    public bool skill_4Rpc
    {
        get { return skill_4; }
        set
        {
            skill_4 = value;
            photonView.RPC("RpcSkill_4", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool attack;
    public bool AttackRpc
    {
        get { return attack; }
        set
        {
            attack = value;
            photonView.RPC("RpcAttack", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool run;
    public bool runRpc
    {
        get { return run; }
        set
        {
            run = value;
            photonView.RPC("RpcRun", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool idle;
    public bool idleRpc
    {
        get { return idle; }
        set
        {
            idle = value;
            photonView.RPC("RpcIdle", Photon.Pun.RpcTarget.All, value);
        }
    }
    bool hit;
    public bool hitRpc
    {
        get { return hit; }
        set
        {
            hit = value;
            photonView.RPC("RpcHit", Photon.Pun.RpcTarget.All, value);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            // stream.SendNext((int)controllerScript._characterState);
            Debug.Log("transform.position - " + transform.position + " - " + transform.rotation);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //  stream.SendNext(transform.localScale);
            //  stream.SendNext(GetComponent<Rigidbody>().velocity);

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

            Debug.Log("correctPlayerPos - " + correctPlayerPos + " - correctPlayerRot - " + correctPlayerRot);

            // correctPlayerScale = (Vector3)stream.ReceiveNext();
            // GetComponent<Rigidbody>().velocity = (Vector3)stream.ReceiveNext();

            /*   controllerScript.attack = (bool)stream.ReceiveNext();
               controllerScript.skill_1 = (bool)stream.ReceiveNext();
               controllerScript.skill_2 = (bool)stream.ReceiveNext();
               controllerScript.skill_3 = (bool)stream.ReceiveNext();
               controllerScript.skill_4 = (bool)stream.ReceiveNext();
               controllerScript.run = (bool)stream.ReceiveNext();
               controllerScript.idle = (bool)stream.ReceiveNext();
               controllerScript.hit = (bool)stream.ReceiveNext();  */


            /*  if (!appliedInitialUpdate)
              {
                  appliedInitialUpdate = true;
                  transform.position = correctPlayerPos;
                  transform.rotation = correctPlayerRot;
                  //  transform.localScale = correctPlayerScale;
                 // GetComponent<Rigidbody>().velocity = Vector3.zero;
              }   */
        }
    }



    private void Singleton_skill4()
    {
        Debug.Log("Skill4");
        skill_4Rpc = true;
    }

    private void Singleton_skill3()
    {
        Debug.Log("Skill3");
        skill_3Rpc = true;
    }

    private void Singleton_skill2()
    {
        Debug.Log("Skill2");
        skill_2Rpc = true;
    }

    private void Singleton_skill1()
    {
        Debug.Log("Skill1");
        skill_1Rpc = true;
    }

    private void Singleton_Attack()
    {
        Debug.Log("Attack");
        AttackRpc = true;
    }

    #region PUN RPC
    [PunRPC]
    public void RpcAttack(bool value)
    {
        attack = value;
    }
    [PunRPC]
    public void RpcIdle(bool value)
    {
        idle = value;
    }
    [PunRPC]
    public void RpcRun(bool value)
    {
        run = value;
    }
    [PunRPC]
    public void RpcHit(bool value)
    {
        hit = value;
    }
    [PunRPC]
    public void RpcSkill_4(bool value)
    {
        skill_4 = value;
    }
    [PunRPC]
    public void RpcSkill_3(bool value)
    {
        skill_3 = value;
    }
    [PunRPC]
    public void RpcSkill_2(bool value)
    {
        skill_2 = value;
    }
    [PunRPC]
    public void RpcSkill_1(bool value)
    {
        skill_1 = value;
    }
    #endregion

    void Update()
    {

        if (!photonView.IsMine)
        {
            Debug.Log("Update   transform.position " + photonView.IsMine + " - " + correctPlayerPos);

            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            /*  transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
              transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);   */
            //     transform.localScale = Vector3.Lerp(transform.localScale, correctPlayerScale, Time.deltaTime * 5);
        }

    }

    float SidestepAxisInput
    // If the right mouse button is held, the horizontal axis also turns into sidestep handling
    {
        get
        {
            if (Input.GetMouseButton(1))
            {
                float sidestep = -(Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? 1 : 0);
                float horizontal = Input.GetAxis("Horizontal");

                return Mathf.Abs(sidestep) > Mathf.Abs(horizontal) ? sidestep : horizontal;
            }
            else
            {
                float sidestep = -(Input.GetKey(KeyCode.Q) ? 1 : 0) + (Input.GetKey(KeyCode.E) ? 1 : 0);
                return sidestep;
            }
        }
    }

    public bool IsMine
    {
        get
        {
            return GetComponent<PhotonView>().IsMine;
        }
    }


    void OnDrawGizmos()
    // Use gizmos to gain information about the state of your setup
    {
        if (!showGizmos || target == null)
        {
            return;
        }

        Gizmos.color = grounded ? Color.blue : Color.red;
        Gizmos.DrawLine(target.transform.position + target.transform.up * -groundedCheckOffset,
            target.transform.position + target.transform.up * -(groundedCheckOffset + groundedDistance));
    }


    public void PositionBy(Vector3 position, Vector3 joystick)
    {
        Debug.Log("PositionBy " + position + "-" + joystick);
        if (joystick != Vector3.zero)
        {
            runRpc = true;
            idleRpc = false;
        }
        else
        {
            runRpc = false;
            idleRpc = true;
        }


        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }

    public void RotateBy(Vector3 moveVector)
    {
        if (moveVector != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveVector);
    }

    public void PlayAnim(string name)
    {
        Debug.Log("Play Anim " + name);
    }
}
