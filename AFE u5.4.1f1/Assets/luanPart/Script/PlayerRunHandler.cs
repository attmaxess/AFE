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

    public class PlayerRunHandler : SkillHandler,
        IChampionTransform
    {
        [SerializeField] private float speedSmooth = 1;

        public float SpeedSmooth => speedSmooth;

        private Vector3 RotateTarget { get; set; }

        public Vector3 Forward
        {
            get { return transform.forward; }
            set { RotateTarget = value; }
        }

        // Use this for initialization
        private void Start()
        {
            JoystickInputFilterObserver?.OnRunAsObservable()
                .Where(_ => IsCanUse())
                .Select(message => message.Direction)
                .DistinctUntilChanged()
                .Subscribe(direction =>
                {
                    transform.position = direction; //* ChampionConfig.MoveSpeed.Value;
                });

            JoystickInputFilterObserver?.OnRunAsObservable()
                .Where(_ => IsCanUse())
                .Select(message => message.Rotation)
                .Where(rotation => rotation != Vector3.zero)
                .Subscribe(rotation => RotateTarget = rotation);

            Observable.EveryUpdate()
                .Where(_ => RotateTarget != Vector3.zero)
                .Subscribe(x =>
                {
                    transform.forward = Vector3.Lerp(transform.forward, RotateTarget, SpeedSmooth);
                    if (transform.forward == RotateTarget)
                        RotateTarget = Vector3.zero;
                });

            JoystickInputFilterObserver?.OnRunAsObservable()
                .Where(_ => IsCanUse())
                .Select(message => IsRun(message.Rotation))
                .DistinctUntilChanged()
                .Subscribe(HandleAnimationRun);
        }

        protected override bool IsCanUse()
        {
            return !AnimationStateChecker.IsInStateSpell1.Value
                   && !AnimationStateChecker.IsInStateSpell2.Value
                   && !AnimationStateChecker.IsInStateSpell3.Value
                   && !AnimationStateChecker.IsInStateSpell4.Value;
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