using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerRunHandle : MonoBehaviour
{
    private IJoystickInputFilterObserver joystickInputFilterObserver;

    private void Awake()
    {
        joystickInputFilterObserver = GetComponent<JoystickInputFilter>();
    }

    // Use this for initialization
    void Start()
    {
        joystickInputFilterObserver.OnRunAsObservable()
            .Subscribe(message => { transform.position = message.Direction; });
    }

    // Update is called once per frame
    void Update()
    {
    }
}