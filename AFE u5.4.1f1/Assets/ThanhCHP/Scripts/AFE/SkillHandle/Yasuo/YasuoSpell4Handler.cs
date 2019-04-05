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

        private float BlowUpTimer => blowUpTimer;

        private SyncTweenRPC SyncTweenRpc { get; set; }

        private FollowObjectBlowUp FollowObjectBlowUp { get; set; }

        private Vector3 OffsetPositionTeleport => offsetPositionTeleport;

        protected override void Awake()
        {
            base.Awake();
            SyncTweenRpc = gameObject.GetOrAddComponent<SyncTweenRPC>();
            FollowObjectBlowUp = gameObject.GetOrAddComponent<FollowObjectBlowUp>();
        }

        protected override void Start()
        {
            base.Start();
            JoystickInputFilterObserver
                .OnSpell4AsObservable()
                .Where(_ => IsCanUse())
                .Subscribe(message =>
                {
                    var newMessage = new InputMessage(GetReceiverHealthLowest(), message.Direction);
                    ActiveSkillCurrent(newMessage, 0);
                });

            var countObjectBlowStream = Observable.EveryUpdate()
                .Select(_ => FollowObjectBlowUp.GetObjectsBlowUp().Count())
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

                    SyncTweenRpc.OnSyncPositionComplete += EnterAnimationState;
                    var posReceiver = receiveDamageables.OrderBy(x => x.GetHealth()).First().GetTransform.position;
                    Teleport(GetPointToTeleport(posReceiver));
                    ChampionTransform.Forward = GetFowardOnTeleport(posReceiver);

                    var rList = receiveDamageables.ToList();
                    Observable.EveryUpdate()
                        .ThrottleFirst(TimeSpan.FromMilliseconds(300))
                        .Take(3)
                        .Subscribe(_ =>
                        {
                            rList.ForEach(receiveDamageable =>
                            {
                                var k = receiveDamageable.GetComponent<IKnockUpable>();
                                k?.BlowUp(BlowUpTimer);
                            });
                        });
                });
            }
        }

        protected override bool IsCanUse()
        {
            return !AnimationStateChecker.IsInStateSpell4.Value;
        }

        private IReceiveDamageable GetReceiverHealthLowest()
        {
            return FollowObjectBlowUp.GetObjectsBlowUpOrderByHealth()
                .FirstOrDefault();
        }

        private void Teleport(Vector3 target)
        {
            SyncTweenRpc.SyncVectorTween(SyncTweenRPC.SyncMode.Position, transform.position, target,
                0.1f, ObservableTween.EaseType.Linear);
        }

        private Vector3 GetPointToTeleport(Vector3 target)
        {
            return new Vector3(target.x, transform.position.y, target.z) +
                   OffsetPositionTeleport;
        }

        private void EnterAnimationState()
        {
            Animator.SetTriggerWithBool(Constant.AnimationPram.R);
            SyncTweenRpc.OnSyncPositionComplete -= EnterAnimationState;
        }

        private Vector3 GetFowardOnTeleport(Vector3 target)
        {
            return (new Vector3(target.x, transform.position.y, target.y) - GetPointToTeleport(target)).normalized;
        }
    }
}