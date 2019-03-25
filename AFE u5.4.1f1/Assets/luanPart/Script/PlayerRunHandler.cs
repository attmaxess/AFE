using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;

public class PlayerRunHandler : MonoBehaviourPun,
    IInitialize<IChampionConfig>
{
    private IJoystickInputFilterObserver joystickInputFilterObserver;

    private IChampionConfig ChampionConfig { get; set; }

    public float speedSmooth = 100;

    private void Awake()
    {
        joystickInputFilterObserver = GetComponent<JoystickInputFilter>();
    }

    void IInitialize<IChampionConfig>.Initialize(IChampionConfig init)
    {
        ChampionConfig = init;
    }

    // Use this for initialization
    void Start()
    {
        Vector3 rotateTarget = Vector3.zero;
        joystickInputFilterObserver.OnRunAsObservable()
            .Subscribe(message =>
            {
                transform.position = message.Direction * ChampionConfig.MoveSpeed.Value;
                rotateTarget = message.Rotation;
                /*     if (message.Rotation != Vector3.zero)
                         transform.rotation = Quaternion.LookRotation(message.Rotation);*/
            });

        Observable.EveryUpdate().Subscribe(x =>
        {
            if (rotateTarget != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTarget), speedSmooth * Time.deltaTime);
        });

    }

    // Update is called once per frame
    void Update()
    {
    }
}