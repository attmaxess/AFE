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
            .Subscribe(dir =>
            {
                transform.position = dir;
            });

    }

    // Update is called once per frame
    void Update()
    {

    }
}

