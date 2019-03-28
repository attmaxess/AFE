using UnityEngine;
using AnimeRx;
using UniRx;
using System.Linq;
using System;

namespace Com.Beetsoft.AFE
{
    public class IMessageBladeAttack
    {
        public bool isUsing;
        public bool isMine;
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
        [SerializeField] private LayerMask layerMaskTarget;

        private LayerMask LayerMaskTarget => layerMaskTarget;
        public float timeMove = 1;
        [Tooltip("milliseconds")]
        public float timeDelayTakeDam = 100;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receiver = gameObject.ReceiverDamageNearestByRayCast(inputMessage.Direction
                , SkillConfig.Range.Value, LayerMaskTarget);
            ActiveSkillSubject.OnNext(new IReceiveDamageable[] { receiver });
            if (receiver == null) return;
            if (photonView.IsMine)
                MessageBroker.Default.Publish<IMessageBladeAttack>(new IMessageBladeAttack(true, photonView.IsMine, transform));

            var posTarget = transform.position + inputMessage.Direction * SkillConfig.Range.Value;
            transform.rotation = Quaternion.LookRotation(inputMessage.Direction);
            ObservableTween.Tween(transform.position, posTarget, timeMove, ObservableTween.EaseType.Linear)
                .DoOnCompleted(() => { if (photonView.IsMine) MessageBroker.Default.Publish<IMessageBladeAttack>(new IMessageBladeAttack(false, photonView.IsMine, transform)); })
         .Subscribe(rate =>
         {
             transform.position = rate;
         });

            Observable.Timer(TimeSpan.FromMilliseconds(timeDelayTakeDam))
                .Subscribe(_ =>
                {
                    //
                    receiver.TakeDamage(new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
                });
        }
    }
}