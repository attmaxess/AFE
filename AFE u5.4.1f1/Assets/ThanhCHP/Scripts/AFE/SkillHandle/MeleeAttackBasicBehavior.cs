using ControlFreak2;
using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicBehavior : SkillBehaviour
    {
        public float rangeDetect = 3;
        public float velocity = 1;
        [SerializeField] private ObservableTween.EaseType easeType;
        [SerializeField] private int millisecondsDelayApplyDamage = 250;
        public Vector3 direction;
        public Vector3 posTarget;
        public IDisposable disposable;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            Debug.Log("ActiveSkill " + inputMessage);
            var receiver = gameObject.GetReceiveDamageableHealthLowest(rangeDetect);
            if (receiver == null)
            {
                ActiveSkillSubject.OnNext(null);
                return;
            }
            float distance = Vector3.Distance(transform.position, receiver.GetTransform.position);

            if (SkillConfig.Range.Value >= distance)
            {
                // attack
                Attack();
            }
            else
            {
                ActiveSkillSubject.OnNext(new[] { receiver, receiver, receiver });
                // move to target and attack
                Vector3 direction = (receiver.GetTransform.position - transform.position).normalized;
                float rangeMove = distance - SkillConfig.Range.Value;
                var position = transform.position;
                posTarget = position + direction * rangeMove;
                disposable?.Dispose();
                MessageBroker.Default.Publish(new IInvertionPositionPlayerJoystic(true, photonView.IsMine, transform));
                disposable = Observable.EveryUpdate().Subscribe(_ =>
               {
                   // transform.position = Vector3.MoveTowards(transform.position, posTarget, Time.deltaTime * ChampionConfig.MoveSpeed.Value);

                   transform.Translate(direction * ChampionConfig.MoveSpeed.Value * Time.deltaTime, Space.World);
                   if (Vector3.Distance(transform.position, posTarget) <= 0.1f)
                   {
                       disposable?.Dispose();
                       transform.position = posTarget;
                       Attack();
                       MessageBroker.Default.Publish(new IInvertionPositionPlayerJoystic(false, photonView.IsMine, transform));
                   }

                   if (CF2Input.GetAxis("Horizontal") != 0 || CF2Input.GetAxis("Vertical") != 0)
                   {
                       disposable?.Dispose();
                       MessageBroker.Default.Publish(new IInvertionPositionPlayerJoystic(false, photonView.IsMine, transform));
                   }
               });
            }
        }

        void Attack()
        {
            var receiver = gameObject.GetReceiveDamageableHealthLowest(SkillConfig.Range.Value);
            if (receiver == null)
            {
                ActiveSkillSubject.OnNext(null);
                return;
            }
            Vector3 direction = (receiver.GetTransform.position - transform.position).normalized;
            ChampionTransform.Forward = direction;
            ActiveSkillSubject.OnNext(new[] { receiver });
            Observable.Timer(TimeSpan.FromMilliseconds(millisecondsDelayApplyDamage))
                .Subscribe(_ =>
                {
                    receiver.TakeDamage(
                        new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
                });
        }
    }
}