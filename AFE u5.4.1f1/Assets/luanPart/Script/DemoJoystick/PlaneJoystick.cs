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
    public ICharacterTranform rotateChar;
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

        GameManagerArVik.Singleton.attack += Singleton_Attack;
        GameManagerArVik.Singleton.skill1 += Singleton_skill1;
        GameManagerArVik.Singleton.skill2 += Singleton_skill2;
        GameManagerArVik.Singleton.skill3 += Singleton_skill3;
        GameManagerArVik.Singleton.skill4 += Singleton_skill4;

        CF2Input.GetButton("Attack");
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

    bool skill_1;
    public bool skill_1Rpc
    {
        get { return skill_1; }
        set
        {
            joystickCharacter.Spell1(new SkillMessage());
            skill_1 = value;
        }
    }
    bool skill_2;
    public bool skill_2Rpc
    {
        get { return skill_2; }
        set
        {
            joystickCharacter.Spell2(new SkillMessage());
            skill_2 = value;
        }
    }
    bool skill_3;
    public bool skill_3Rpc
    {
        get { return skill_3; }
        set
        {
            joystickCharacter.Spell3(new SkillMessage());
            skill_3 = value;
        }
    }
    bool skill_4;
    public bool skill_4Rpc
    {
        get { return skill_4; }
        set
        {
            joystickCharacter.Spell4(new SkillMessage());
            skill_4 = value;
        }
    }
    bool attack;
    public bool AttackRpc
    {
        get { return attack; }
        set
        {
            joystickCharacter.BasicAttack(new SkillMessage());
            attack = value;
        }
    }
    bool run;
    public bool runRpc
    {
        get { return run; }
        set
        {
            run = value;
        }
    }
    bool idle;
    public bool idleRpc
    {
        get { return idle; }
        set
        {
            idle = value;
        }
    }
    bool hit;
    public bool hitRpc
    {
        get { return hit; }
        set
        {
            hit = value;
        }
    }

    void Update()
    {
        if (!useUpdate) return;

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
            //  joystickCharacter.Spell1(new SkillMessage());
            Debug.Log("-" + CF2Input.GetButton("Skill1") + CF2Input.GetButtonDown("Skill1") + CF2Input.GetButtonUp("Skill1"));

        }
        if ((CF2Input.GetAxis("S_2_Hoz") != 0 || CF2Input.GetAxis("S_2_Ver") != 0) && CF2Input.GetButton("Skill2"))
        {
            h1 = CF2Input.GetAxis("S_2_Hoz");
            v1 = CF2Input.GetAxis("S_2_Ver");
            //joystickCharacter.Spell2(new SkillMessage());
        }
        if ((CF2Input.GetAxis("S_3_Hoz") != 0 || CF2Input.GetAxis("S_3_Ver") != 0) && CF2Input.GetButton("Skill3"))
        {
            h1 = CF2Input.GetAxis("S_3_Hoz");
            v1 = CF2Input.GetAxis("S_3_Ver");
            //  joystickCharacter.Spell3(new SkillMessage());
        }
        if ((CF2Input.GetAxis("S_4_Hoz") != 0 || CF2Input.GetAxis("S_4_Ver") != 0) && CF2Input.GetButton("Skill4"))
        {
            h1 = CF2Input.GetAxis("S_4_Hoz");
            v1 = CF2Input.GetAxis("S_4_Ver");
            // joystickCharacter.Spell4(new SkillMessage());
        }

        if (CF2Input.GetButtonUp("Skill1"))
        {
            Debug.Log("-" + CF2Input.GetButtonUp("Skill1"));
            joystickCharacter.Spell1(new SkillMessage());
        }

        if (CF2Input.GetButtonUp("Skill2"))
        {
            Debug.Log("-" + CF2Input.GetButtonUp("Skill2"));
            joystickCharacter.Spell2(new SkillMessage());
        }


        if (CF2Input.GetButtonUp("Skill3"))
        {
            Debug.Log("-" + CF2Input.GetButtonUp("Skill3"));
            joystickCharacter.Spell3(new SkillMessage());
        }

        if (CF2Input.GetButtonUp("Skill4"))
        {
            Debug.Log("-" + CF2Input.GetButtonUp("Skill4"));
            joystickCharacter.Spell4(new SkillMessage());
        }

        Vector3 dirSkill = (transform.right * h1 + transform.forward * v1);

        directionSkill.localPosition = new Vector3(h1 / 2, 0.5f, v1 / 2);

        //----------

        if (joystickCharacter != null)
        {

            //   rotateChar.RotateBy(moveVector);

            // rotateChar.PositionBy(transform.position, moveVector);

            if (moveVector == Vector3.zero)
                joystickCharacter.Idle(new RunMessage(transform.position, Vector3.zero));
            else
                joystickCharacter.Run(new RunMessage(transform.position, moveVector));

            //   transform.position = rotateChar.transform.position;
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

    }
}

public interface IPlaneJoystickTranform
{
    Vector3 directionRotate { get; }
}

public interface ICharacterTranform
{
    void PositionBy(Vector3 position, Vector3 joystick);
    void RotateBy(Vector3 moveVector);
    Transform transform { get; }
    bool IsMine { get; }
    bool InCamera { get; set; }
}
