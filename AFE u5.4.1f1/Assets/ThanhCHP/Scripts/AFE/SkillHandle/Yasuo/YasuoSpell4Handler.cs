using System;
using System.Collections.Generic;
using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell4Handler : SkillHandler, ISkillSpell_4
    {
        [SerializeField] private float blowUpTimer = 0.7f;
        [SerializeField] private Vector3 offsetPositionTeleport = Vector3.forward;
        private HashSet<IReceiveDamageable> ObjectBlowUps { get; } = new HashSet<IReceiveDamageable>();

        private float BlowUpTimer => blowUpTimer;

        private SyncTweenRPC SyncTweenRpc { get; set; }

        private Vector3 OffsetPositionTeleport => offsetPositionTeleport;

        protected override void Awake()
        {
            base.Awake();
            SyncTweenRpc = gameObject.GetOrAddComponent<SyncTweenRPC>();
        }

        protected override void Start()
        {
            base.Start();
            JoystickInputFilterObserver
                .OnSpell4AsObservable()
                .Where(_ => !AnimationStateChecker.IsInStateSpell4.Value)
                .Subscribe(message =>
                {
                    var newMessage = new InputMessage(GetReceiverHealthLowest(), message.Direction);
                    ActiveSkillCurrent(newMessage, 100);
                });

            AsyncMessageBroker.Default.Subscribe<BlockUpArgs>(message =>
            {
                return Observable.Return(message.ObjectHasBeenBlowUp)
                    .ForEachAsync(obj => { ObjectBlowUps.Add(obj); });
            });

            AsyncMessageBroker.Default.Subscribe<BlockDownArgs>(args =>
            {
                return Observable.Return(args.ObjectHasBeenBlowDown)
                    .ForEachAsync(obj => { ObjectBlowUps.Remove(obj); });
            });

            var countObjectBlowStream = Observable.EveryUpdate()
                .Select(_ => ObjectBlowUps.Count)
                .DistinctUntilChanged();

            countObjectBlowStream
                .Where(x => x > 0)
                .Subscribe(_ =>
                {
                    SkillReader.SendNext(1);
                    InitValue(0);
                });

            countObjectBlowStream
                .Where(x => x == 0)
                .Subscribe(_ =>
                {
                    SkillReader.SendNextFirstIndex();
                    InitValue(0);
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
                .Select(x => x.OnActiveSkillAsObservable()))
            {
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    if (receiveDamageables.IsNullOrEmpty()) return;

                    var posReceiver = receiveDamageables.First().GetTransform.position;
                    var posTarget = new Vector3(posReceiver.x, transform.position.y, posReceiver.z) +
                                    OffsetPositionTeleport;
                    SyncTweenRpc.SyncVectorTween(SyncTweenRPC.SyncMode.Position, transform.position, posTarget,
                        0.1f, ObservableTween.EaseType.Linear);
                    receiveDamageables.ToList().ForEach(receiveDamageable =>
                    {
                        var k = receiveDamageable.GetComponent<IKnockUpable>();
                        k?.BlowUp(BlowUpTimer);
                    });

                    Observable.Timer(TimeSpan.FromSeconds(0.1f))
                        .Subscribe(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.R));
                });
            }
        }

        private IReceiveDamageable GetReceiverHealthLowest()
        {
            return ObjectBlowUps
                .OrderBy(x => x.GetHealth())
                .FirstOrDefault();
        }
    }
}