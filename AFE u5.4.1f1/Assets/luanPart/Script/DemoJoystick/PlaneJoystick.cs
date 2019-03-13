using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaneJoystick : MonoBehaviour, IPlaneJoystickTranform
{
    [Header("Debug")]
    public bool isDebug = false;

    [Header("Process")]
    public bool useUpdate = true;

    [Header("Input")]
    public float speed = 1f;
    public Joystick joystick = null;
    public Transform child = null;
    public ICharacterTranform rotateChar = null;

    [Header("Grounded")]
    public bool grounded = false;
    public Rigidbody target;
    public LayerMask groundLayers = -1;
    public float groundedDistance = 9999f;

    public Vector3 directionRotate
    {
        get
        {
            if (joystick != null)
                return transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
            return Vector3.zero;
        }
    }

    [Header("FindRotateChar ")]
    public bool doneFindRotateChar = true;

    IEnumerator FindRotateChar(float delay = 1f)
    {
        yield return new WaitForSeconds(delay);

        if (!doneFindRotateChar) yield break;
        doneFindRotateChar = false;

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

        doneFindRotateChar = true;

        yield break;
    }

    void Start()
    {
        if (joystick == null)
            joystick = Joystick.Singleton;

        if (rotateChar != null)
        {
            StartCoroutine(FindRotateChar(0.5f));
        }
    }

    void Update()
    {
        if (!useUpdate) return;
        if (Camera.main == null) return;

        Vector3 moveVector = (transform.right * joystick.Horizontal + transform.forward * joystick.Vertical);

        var rot = Quaternion.LookRotation(Camera.main.transform.forward);

        transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        child.localPosition = new Vector3(joystick.Horizontal / 2, child.localPosition.y, joystick.Vertical / 2);

        if (rotateChar != null)
        {
            rotateChar.RotateBy(moveVector);
            rotateChar.PositionBy(transform.position);
        }
        else
        {
            StartCoroutine(FindRotateChar(1f));
        }
    }

    void FixedUpdate()
    // Handle movement here since physics will only be calculated in fixed frames anyway
    {
        RaycastHit hitAbove = new RaycastHit();
        RaycastHit hitBelow = new RaycastHit();
        bool boolHitAbove = Physics.Raycast(target.transform.position, target.transform.up, out hitAbove, groundedDistance, groundLayers);
        bool boolHitBelow = Physics.Raycast(target.transform.position, -target.transform.up, out hitBelow, groundedDistance, groundLayers);
        // Shoot a ray downward to see if we're touching the ground

        if (!grounded)
        {
            if (boolHitAbove || boolHitBelow)
            {
                if (boolHitAbove && !boolHitBelow)
                    transform.position = hitAbove.point;
                else if (!boolHitAbove && boolHitBelow)
                    transform.position = hitBelow.point;
                else if (boolHitAbove && boolHitBelow)
                {
                    transform.position = (hitAbove.point - transform.position).magnitude <= (hitBelow.point - transform.position).magnitude ? hitAbove.point : hitBelow.point;
                }

                grounded = true;
            }
        }
        else
        {
            grounded = false;
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
