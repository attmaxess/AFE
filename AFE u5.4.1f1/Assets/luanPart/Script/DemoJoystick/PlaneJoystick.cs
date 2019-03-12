using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaneJoystick : MonoBehaviour, IPlaneJoystickTranform
{
    public float speed = 1;
    public Joystick joystick;
    public Transform child;
    public ICharacterTranform rotateChar;

    public Vector3 directionRotate
    {
        get
        {
            if (joystick != null)
                return transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
            return Vector3.zero;
        }
    }

    void Start()
    {
        if (joystick == null)
            joystick = Joystick.Singleton;
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
    }

    void Update()
    {
        Vector3 moveVector = (transform.right * joystick.Horizontal + transform.forward * joystick.Vertical);

        var rot = Quaternion.LookRotation(Camera.main.transform.forward);

        transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);

        child.localPosition = new Vector3(joystick.Horizontal / 2, 0.5f, joystick.Vertical / 2);

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
