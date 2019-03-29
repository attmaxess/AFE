using AFE.Extensions;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IChampionTransform
    {
        Vector3 Forward { get; set; }
    }

    public class PlayerRunHandler : MonoBehaviourPun,
        IInitialize<IChampionConfig>,
        IInitialize<IJoystickInputFilterObserver>,
        IChampionTransform
    {
        [SerializeField] private float speedSmooth = 1;
        private IJoystickInputFilterObserver JoystickInputFilterObserver { get; set; }

        private IChampionConfig ChampionConfig { get; set; }

        private Animator Animator { get; set; }

        public float SpeedSmooth => speedSmooth;

        private Vector3 RotateTarget { get; set; }

        public Vector3 Forward
        {
            get { return transform.forward; }
            set { RotateTarget = value; }
        }

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
            JoystickInputFilterObserver.OnRunAsObservable()
                .Select(message => message.Direction)
                .DistinctUntilChanged()
                .Subscribe(direction =>
                {
                    transform.position = direction; //* ChampionConfig.MoveSpeed.Value;
                });

            JoystickInputFilterObserver.OnRunAsObservable()
                .Select(message => message.Rotation)
                .Where(rotation => rotation != Vector3.zero)
                .Subscribe(rotation => RotateTarget = rotation);

            Observable.EveryUpdate()
                .Where(_ => RotateTarget != Vector3.zero)
                .Subscribe(x => { transform.forward = Vector3.Lerp(transform.forward, RotateTarget, SpeedSmooth); });

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
}