using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ControlFreak2;


public class PlaneJoystick : MonoBehaviour, IPlaneJoystickTranform
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Process")]
    public bool useUpdate = true;

    [Header("Input")]
    public float speed = 1;
    public Transform child;
    public ICharacterTranform rotateChar;

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
        if (rotateChar != null)
        {
            var mObjs = GameObject.FindObjectsOfType<MonoBehaviour>();
            ICharacterTranform[] interfaceScripts = (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(ICharacterTranform)) select (ICharacterTranform)a).ToArray();
            if (interfaceScripts.Length > 0)
            {
                for (int i = 0; i < interfaceScripts.Length; i++)
                {
                    if (interfaceScripts[i].IsMine)
                    {
                        rotateChar = interfaceScripts[i];
                        break;
                    }
                }
                if (rotateChar == null)
                {
                    Debug.Log("Dont Have Local Character");
                }
            }
            else
            {
                Debug.Log("Dont Find Any Gameobject Have ICharacterTranform");
            }
        }

        CF2Input.GetButton("Attack");
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

        child.localPosition = new Vector3(h / 2, 0.5f, v / 2);

        if (rotateChar != null)
        {

            rotateChar.RotateBy(moveVector);

            rotateChar.PositionBy(transform.position);

            //   transform.position = rotateChar.transform.position;
        }
        else
        {
            var mObjs = GameObject.FindObjectsOfType<MonoBehaviour>();
            ICharacterTranform[] interfaceScripts = (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(ICharacterTranform)) select (ICharacterTranform)a).ToArray();
            if (interfaceScripts.Length > 0)
            {
                for (int i = 0; i < interfaceScripts.Length; i++)
                {
                    if (interfaceScripts[i].IsMine)
                    {
                        rotateChar = interfaceScripts[i];
                        break;
                    }
                }
                if (rotateChar == null)
                {
                    Debug.Log("Dont Have Local Character");
                }
            }
            else
            {
                Debug.Log("Dont Find Any Gameobject Have ICharacterTranform");
            }
        }

        if (CF2Input.GetButtonDown("Pause"))
        {
            Debug.Log("Pause");
        }
        if (CF2Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
        }

        if (CF2Input.GetButtonDown("Skill1"))
        {
            Debug.Log("Skill1");
        }
        if (CF2Input.GetButtonDown("Skill2"))
        {
            Debug.Log("Skill4");
        }
        if (CF2Input.GetButtonDown("Skill3"))
        {
            Debug.Log("Skill4");
        }
        if (CF2Input.GetButtonDown("Skill4"))
        {
            Debug.Log("Skill4");
        }

    }
}

public interface IPlaneJoystickTranform
{
    Vector3 directionRotate { get; }
}

public interface ICharacterTranform
{
    void PositionBy(Vector3 position);
    void RotateBy(Vector3 moveVector);
    Transform transform { get; }
    bool IsMine { get; }
    bool InCamera { get; set; }
}
