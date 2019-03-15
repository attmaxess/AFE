using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChar : MonoBehaviour, ICharacterTranform
{
    public float speed = 1;

    public bool IsMine
    {
        get
        {
            return false;
        }
    }

    public bool InCamera
    {
        get;
        set;
    }

    public void RotateBy(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void PositionBy(Vector3 pos, Vector3 joystick)
    {
        transform.position = pos;
    }

    private void Update()
    {
        //  Vector3 moveVector = (transform.right * Joystick.Singleton.Horizontal + transform.forward * Joystick.Singleton.Vertical);
        //  transform.Translate(moveVector * speed * Time.deltaTime, Space.World);

    }
}
