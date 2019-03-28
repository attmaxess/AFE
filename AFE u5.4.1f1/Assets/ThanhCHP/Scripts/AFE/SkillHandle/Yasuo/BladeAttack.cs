using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class IMessageBladeAttack
    {
        public bool isMine;
        public bool isUsing;
        public Transform player;

        public IMessageBladeAttack(bool isUsing, bool isMine, Transform player)
        {
            this.isUsing = isUsing;
            this.isMine = isMine;
            this.player = player;
        }
    }

    public class BladeAttack : SkillBehaviour
    {
        [SerializeField] private bool isMustDetectTarget;
        [SerializeField] private LayerMask layerMaskTarget;
        [Tooltip("milliseconds")] public float timeDelayTakeDam = 100;
        public float timeMove = 1;
        [SerializeField] private ObservableTween.EaseType easeType;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        private bool IsMustDetectTarget => isMustDetectTarget;

        private ObservableTween.EaseType EaseType => easeType;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            if (IsMustDetectTarget)
            {
                var receiver = gameObject.ReceiverDamageNearestByRayCast(inputMessage.Direction
                    , SkillConfig.Range.Value, LayerMaskTarget);
                if (receiver == null)
                {
                    ActiveSkillSubject.OnNext(null);
                    return;
                }

                ActiveSkillSubject.OnNext(new[] {receiver});

                Observable.Timer(TimeSpan.FromMilliseconds(timeDelayTakeDam))
                    .Subscribe(_ =>
                    {
                        receiver.TakeDamage(
                            new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
                    });
                Dash(inputMessage.Direction);
            }
            else
            {
                ActiveSkillSubject.OnNext(null);
                Dash(inputMessage.Direction);
            }
        }

        private void Dash(Vector3 direction)
        {
            if (photonView.IsMine)
                MessageBroker.Default.Publish(new IMessageBladeAttack(true, photonView.IsMine,
                    transform));
            var posTarget = transform.position + direction * SkillConfig.Range.Value;
            transform.forward = direction;
            ObservableTween.Tween(transform.position, posTarget, timeMove, EaseType)
                .DoOnCompleted(() =>
                {
                    if (photonView.IsMine)
                        MessageBroker.Default.Publish(
                            new IMessageBladeAttack(false, photonView.IsMine, transform));
                })
                .Subscribe(rate => { transform.position = rate; });
        }
    }
}