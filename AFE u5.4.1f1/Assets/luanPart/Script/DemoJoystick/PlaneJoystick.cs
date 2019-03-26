using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ControlFreak2;
using Com.Beetsoft.AFE;
using Photon.Pun;

public class PlaneJoystick : MonoBehaviour, IPlaneJoystickTranform
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Process")]
    public bool useUpdate = true;

    [Header("Input")]
    public float speed = 1;
    public Transform directionPlayer;
    public Transform directionSkill;
    public IJoystickInputFilter joystickCharacter;    

    public Vector3 directionRotate
    {
        get
        {
            var h = CF2Input.GetAxis("Horizontal");
            var v = CF2Input.GetAxis("Vertical");
            return transform.right * h + transform.forward * v;
        }
    }

    void Start()
    {
        if (joystickCharacter != null)
        {
            var mObjs = GameObject.FindObjectsOfType<MonoBehaviour>();
            IJoystickInputFilter[] interfaceScripts = (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(IJoystickInputFilter)) select (IJoystickInputFilter)a).ToArray();
            if (interfaceScripts.Length > 0)
            {
                for (int i = 0; i < interfaceScripts.Length; i++)
                {
                    var mono = interfaceScripts[i] as MonoBehaviour;
                    if (mono != null && mono.GetComponent<PhotonView>().IsMine)
                    {
                        joystickCharacter = interfaceScripts[i];
                        break;
                    }
                }
                if (joystickCharacter == null)
                {
                    Debug.Log("Dont Have Local joystickCharacter");
                }
            }
            else
            {
                Debug.Log("Dont Find Any Gameobject Have joystickCharacter");
            }

        }

        //GameManagerArVik.Singleton.attack += Singleton_Attack;
        //GameManagerArVik.Singleton.skill1 += Singleton_skill1;
        //GameManagerArVik.Singleton.skill2 += Singleton_skill2;
        //GameManagerArVik.Singleton.skill3 += Singleton_skill3;
        //GameManagerArVik.Singleton.skill4 += Singleton_skill4;

    }

    private void Singleton_Attack()
    {
        joystickCharacter.BasicAttack(new InputMessage());
    }


    float previousV1_Skill, previousH1_Skill;

    void Update()
    {
        if (!useUpdate) return;

        if (CF2Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
        }
        if (CF2Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
            Singleton_Attack();
        }


        var h = CF2Input.GetAxis("Horizontal");
        var v = CF2Input.GetAxis("Vertical");
        Vector3 moveVector = (transform.right * h + transform.forward * v);

        var rot = Quaternion.LookRotation(Camera.main.transform.forward);

        transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);

        directionPlayer.localPosition = new Vector3(h / 2, 0.5f, v / 2);

        // direction skill
        float h1 = 0, v1 = 0;

        if ((CF2Input.GetAxis("S_1_Hoz") != 0 || CF2Input.GetAxis("S_1_Ver") != 0) && CF2Input.GetButton("Skill1"))
        {
            h1 = CF2Input.GetAxis("S_1_Hoz");
            v1 = CF2Input.GetAxis("S_1_Ver");

        }
        if ((CF2Input.GetAxis("S_2_Hoz") != 0 || CF2Input.GetAxis("S_2_Ver") != 0) && CF2Input.GetButton("Skill2"))
        {
            h1 = CF2Input.GetAxis("S_2_Hoz");
            v1 = CF2Input.GetAxis("S_2_Ver");
        }
        if ((CF2Input.GetAxis("S_3_Hoz") != 0 || CF2Input.GetAxis("S_3_Ver") != 0) && CF2Input.GetButton("Skill3"))
        {
            h1 = CF2Input.GetAxis("S_3_Hoz");
            v1 = CF2Input.GetAxis("S_3_Ver");
        }
        if ((CF2Input.GetAxis("S_4_Hoz") != 0 || CF2Input.GetAxis("S_4_Ver") != 0) && CF2Input.GetButton("Skill4"))
        {
            h1 = CF2Input.GetAxis("S_4_Hoz");
            v1 = CF2Input.GetAxis("S_4_Ver");
        }
        Vector3 dirSkill = (transform.right * previousH1_Skill + transform.forward * previousV1_Skill);


        if (CF2Input.GetButtonUp("Skill1") && joystickCharacter != null)
        {
            joystickCharacter.Spell1(new InputMessage(dirSkill));
        }

        if (CF2Input.GetButtonUp("Skill2") && joystickCharacter != null)
        {
            joystickCharacter.Spell2(new InputMessage(dirSkill));
        }


        if (CF2Input.GetButtonUp("Skill3") && joystickCharacter != null)
        {
            joystickCharacter.Spell3(new InputMessage(dirSkill));
        }

        if (CF2Input.GetButtonUp("Skill4") && joystickCharacter != null)
        {
            joystickCharacter.Spell4(new InputMessage(dirSkill));
        }


        directionSkill.localPosition = new Vector3(previousH1_Skill / 2, 0.5f, previousV1_Skill / 2);

        //----------

        if (joystickCharacter != null)
        {
//            if (moveVector == Vector3.zero)
//                joystickCharacter.Idle(new RunMessage(transform.position, Vector3.zero));
//            else
                joystickCharacter.Run(new RunMessage(transform.position, moveVector));

        }
        else
        {
            var mObjs = GameObject.FindObjectsOfType<MonoBehaviour>();
            IJoystickInputFilter[] interfaceScripts = (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(IJoystickInputFilter)) select (IJoystickInputFilter)a).ToArray();
            if (interfaceScripts.Length > 0)
            {
                for (int i = 0; i < interfaceScripts.Length; i++)
                {
                    var mono = interfaceScripts[i] as MonoBehaviour;
                    if (mono != null && mono.GetComponent<PhotonView>().IsMine)
                    {
                        joystickCharacter = interfaceScripts[i];
                        break;
                    }
                }
                if (joystickCharacter == null)
                {
                    Debug.Log("Dont Have Local joystickCharacter");
                }
            }
            else
            {
                Debug.Log("Dont Find Any Gameobject Have joystickCharacter");
            }

        }

        previousV1_Skill = v1;
        previousH1_Skill = h1;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

public interface IPlaneJoystickTranform
{
    Vector3 directionRotate { get; }
    Transform GetTransform();
}

public interface ICharacterTranform
{
    void PositionBy(Vector3 position, Vector3 joystick);
    void RotateBy(Vector3 moveVector);
    Transform transform { get; }
    bool IsMine { get; }
    bool InCamera { get; set; }
}
