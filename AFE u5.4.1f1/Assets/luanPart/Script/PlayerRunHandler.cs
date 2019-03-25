using AFE.Extensions;
using Com.Beetsoft.AFE;
using Photon.Pun;
using UniRx;
using UnityEngine;

public class PlayerRunHandler : MonoBehaviourPun,
    IInitialize<IChampionConfig>,
    IInitialize<IJoystickInputFilterObserver>
{
    public float speedSmooth = 100;
    private IJoystickInputFilterObserver JoystickInputFilterObserver { get; set; }

    private IChampionConfig ChampionConfig { get; set; }

    private Animator Animator { get; set; }

    void IInitialize<IChampionConfig>.Initialize(IChampionConfig init)
    {
        ChampionConfig = init;
    }

    void IInitialize<IJoystickInputFilterObserver>.Initialize(IJoystickInputFilterObserver init)
    {
        JoystickInputFilterObserver = init;
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    // Use this for initialization
    private void Start()
    {
        var rotateTarget = Vector3.zero;
        JoystickInputFilterObserver.OnRunAsObservable()
            .Subscribe(message =>
            {
                transform.position = message.Direction * ChampionConfig.MoveSpeed.Value;
                rotateTarget = message.Rotation;
            });

        Observable.EveryUpdate().Subscribe(x =>
        {
            if (rotateTarget != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateTarget),
                    speedSmooth * Time.deltaTime);
        });

        JoystickInputFilterObserver.OnRunAsObservable()
            .Select(message => IsRun(message.Rotation))
            .DistinctUntilChanged()
            .Subscribe(HandleAnimationRun);
    }

    private void HandleAnimationRun(bool isRun)
    {
        Animator.SetTriggerWithBool(isRun ? Constant.AnimationPram.Run : Constant.AnimationPram.Idle);
    }

    private bool IsRun(Vector3 direction)
    {
        return direction != Vector3.zero;
    }
}