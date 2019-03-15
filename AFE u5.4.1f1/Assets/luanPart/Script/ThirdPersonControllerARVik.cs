using System;
using UnityEngine;

/// <summary>
/// ko dung script nay nua. su dung ThirdPersonControllerNET.cs
/// </summary>

public class ThirdPersonControllerARVik : MonoBehaviour, ICharacterTranform
{
    public Rigidbody target;

    public float groundedCheckOffset = 0.7f;
    public bool
        showGizmos = true,
        requireLock = true,
        controlLock = false;

    private const float groundedDistance = 0.5f;


    private bool grounded, walking;


    public bool Grounded
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

    void Reset()
    {
        Setup();
    }


    void Setup()
    {
        if (target == null)
        {
            target = GetComponent<Rigidbody>();
        }
    }


    void Start()
    {
        Setup();
    }

    void Update()
    {


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

    public void SetIsRemotePlayer(bool v)
    {
    }

    public bool IsMine { get { return GetComponent<PhotonView>().isMine; } }

    public void PositionBy(Vector3 position, Vector3 joystick)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }

    public void RotateBy(Vector3 moveVector)
    {
        transform.rotation = Quaternion.LookRotation(moveVector);
    }
}
