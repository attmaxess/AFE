using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ControlFreak2;
using Com.Beetsoft.AFE;
using Photon.Pun;
using UniRx;
using System;

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
    private bool IsUpdateWhenSkill;

    public GameObject mainCharacter;
    public ICrowdControl crowdControl;
    public bool isCrowdConttroll;
    public bool isDeath;
    public IAnimationStateChecker animationStateChecker;

    public bool useSkill_1;
    public bool useSkill_2;
    public bool useSkill_3;
    public bool useSkill_4;
    public bool useBasicAttack;
    public bool waitCountTime;
    List<TestYasuo> testYasuos = new List<TestYasuo>();

    public Vector3 directionRotate
    {
        get
        {
            var h = CF2Input.GetAxis("Horizontal");
            var v = CF2Input.GetAxis("Vertical");
            return transform.right * h + transform.forward * v;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => mainCharacter != null);

        if (crowdControl == null)
        {
            crowdControl = mainCharacter?.GetComponent<ICrowdControl>();
            if (joystickCharacter == null)
                Debug.Log("Dont Find Any Gameobject Have ICrowdControl");
            crowdControl?.IsCrowdControl.Subscribe(isCrowdConttroll =>
            {
                this.isCrowdConttroll = isCrowdConttroll;
            });
        }
        animationStateChecker = mainCharacter.GetComponent<IAnimationStateChecker>();

        animationStateChecker?.IsBasicAttack.Subscribe(use =>
        {
            useBasicAttack = use;
        });
        animationStateChecker?.IsInStateSpell1.Subscribe(use =>
        {
            useSkill_1 = use;
        });
        animationStateChecker?.IsInStateSpell2.Subscribe(use =>
        {
            useSkill_2 = use;
        });
        animationStateChecker?.IsInStateSpell3.Subscribe(use =>
        {
            useSkill_3 = use;
        });
        animationStateChecker?.IsInStateSpell4.Subscribe(use =>
        {
            useSkill_4 = use;
        });

        if (joystickCharacter == null)
        {
            joystickCharacter = mainCharacter?.GetComponent<IJoystickInputFilter>();
            if (joystickCharacter == null)
                Debug.Log("Dont Find Any Gameobject Have joystickCharacter");
        }

        mainCharacter?.GetComponent<TestYasuo>().ChampionModel.Health.Subscribe(health =>
        {
            if (health <= 0)
            {
                isDeath = true;
            }
            else
            {
                isDeath = false;
            }
        });

        speed = mainCharacter.GetComponent<TestYasuo>().ChampionModel.MoveSpeed.Value;

        MessageBroker.Default.Receive<IInvertionPositionPlayerJoystic>().Subscribe(mes =>
        {
            transform.position = mes.player.position;
            IsUpdateWhenSkill = mes.isUsing;
        });

        MessageBroker.Default.Receive<MessageChangedCharacterYasuo>().Subscribe(mess =>
        {
            if (mess.addOrRemove)
            {
                testYasuos.Add(mess.yasuo);
            }
            else
            {
                testYasuos.Remove(mess.yasuo);
            }
            if (testYasuos.Count == 1)
            {
            }
            if (testYasuos.Count == 2)
            {
                waitCountTime = true;
                int count = 3;
                Observable.Interval(System.TimeSpan.FromSeconds(1)).TakeWhile(_ => count >= 0 && testYasuos.Count == 2).Subscribe(_ =>
                {
                    if (count <= 1)
                    {
                        waitCountTime = false;
                    }
                    count--;
                });
            }
            if (testYasuos.Count == 0 || testYasuos.Count >= 3)
            {
            }
        });

        Observable.EveryLateUpdate().TakeUntilDestroy(gameObject).Subscribe(_ =>
        {
            if (mainCharacter == null) Destroy(gameObject);
        });

        yield break;
    }


    float previousV1_Skill, previousH1_Skill;

    void Update()
    {
        if (mainCharacter == null) return;

        if (!useUpdate || IsUpdateWhenSkill) return;

        if (CF2Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
        }

        if (waitCountTime) return;

        if (isDeath) return;

        if (isCrowdConttroll) return;


        if (CF2Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
            joystickCharacter.BasicAttack(new InputMessage());
        }



        var h = CF2Input.GetAxis("Horizontal");
        var v = CF2Input.GetAxis("Vertical");
        Vector3 moveVector = Vector3.zero;
        if (useSkill_1 || useSkill_2 || useSkill_3 || useSkill_4 || useBasicAttack)
        {
        }
        else
        {
            moveVector = (transform.right * h + transform.forward * v);

            var rot = Quaternion.LookRotation(Camera.main.transform.forward);

            transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

            transform.Translate(moveVector.normalized * speed * Time.deltaTime, Space.World);

            directionPlayer.localPosition = new Vector3(h / 2, 0.5f, v / 2);
        }
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
        Vector3 dirSkill = (transform.right * previousH1_Skill + transform.forward * previousV1_Skill).normalized;


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
            joystickCharacter.Run(new RunMessage(transform.position, moveVector));
        }
        else
        {
            joystickCharacter = mainCharacter?.GetComponent<IJoystickInputFilter>();
            if (joystickCharacter == null)
                Debug.Log("Dont Find Any Gameobject Have joystickCharacter");
        }

        if (crowdControl == null)
        {
            crowdControl = mainCharacter?.GetComponent<ICrowdControl>();
            if (joystickCharacter == null)
                Debug.Log("Dont Find Any Gameobject Have ICrowdControl");
            crowdControl?.IsCrowdControl.Subscribe(isCrowdConttroll =>
            {
                this.isCrowdConttroll = isCrowdConttroll;
            });
        }

        previousV1_Skill = v1;
        previousH1_Skill = h1;
    }

    internal void SetMainCharacter(GameObject newChar)
    {
        mainCharacter = newChar;
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
