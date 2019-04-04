using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicBehavior : SkillBehaviour
    {
        public float rangeDetect = 3;
        private SyncTweenRPC SyncTweenRpc { get; set; }
        public float velocity = 1;
        [SerializeField] private ObservableTween.EaseType easeType;
        [SerializeField] private int millisecondsDelayApplyDamage = 250;
        public Vector3 direction;
        public Vector3 posTarget;
        protected override void Awake()
        {
            base.Awake();
            SyncTweenRpc = gameObject.GetOrAddComponent<SyncTweenRPC>();
        }

        private void Start()
        {
            SyncTweenRpc.OnSyncPositionComplete += Attack;
            SyncTweenRpc.OnSyncPositionComplete += SetExactPos;
        }

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receiver = gameObject.GetReceiveDamageableHealthLowest(rangeDetect);
            if (receiver == null)
            {
                ActiveSkillSubject.OnNext(null);
                return;
            }
            float distance = Vector3.Distance(transform.position, receiver.GetTransform.position);

            Debug.Log("distance " + distance + " - " + SkillConfig.Range.Value + " - " + (SkillConfig.Range.Value >= distance));

            if (SkillConfig.Range.Value >= distance)
            {
                // attack
                Attack();
            }
            else
            {
                ActiveSkillSubject.OnNext(new[] { receiver, receiver, receiver });
                // move to target and attack
                Vector3 direction = receiver.GetTransform.position - transform.position;
                float rangeMove = distance - SkillConfig.Range.Value;
                var position = transform.position;
                posTarget = position + direction.normalized * rangeMove;
                SyncTweenRpc.SyncVectorTween(SyncTweenRPC.SyncMode.Position, position, posTarget, velocity / distance, easeType);
            }
        }

        void Attack()
        {
            var receiver = GetComponent<TestYasuo>()?.centerCharacter.gameObject.ReceiverDamageNearestByRayCastAll(transform.forward, SkillConfig.Range.Value);
            if (receiver == null)
            {
                ActiveSkillSubject.OnNext(null);
                return;
            }
            ActiveSkillSubject.OnNext(new[] { receiver });
            Observable.Timer(TimeSpan.FromMilliseconds(millisecondsDelayApplyDamage))
                .Subscribe(_ =>
                {
                    receiver.TakeDamage(
                        new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
                });
        }

        void SetExactPos()
        {
            transform.position = posTarget;
        }
    }
}