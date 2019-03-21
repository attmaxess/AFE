using Com.Beetsoft.AFE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;

public class PlayerRunHandle : MonoBehaviourPun,
    IInitialize<IChampionConfig>
{
    private IJoystickInputFilterObserver joystickInputFilterObserver;

    private IChampionConfig ChampionConfig { get; set; }

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
        joystickInputFilterObserver.OnRunAsObservable()
            .Subscribe(message =>
            {
                transform.position = message.Direction * ChampionConfig.MoveSpeed.Value;
                if (message.Rotation != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(message.Rotation);
            });
    }

    // Update is called once per frame
    void Update()
    {
    }
}