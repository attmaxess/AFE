using System;
using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using Photon.Pun;
using UniRx;
using UnityEngine;
using AnimationState = Com.Beetsoft.AFE.Enumerables.AnimationState;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1Handler : SkillHandler, ISkillSpell_1
    {
        [SerializeField] private float timeKnockUpObject = 0.7f;
        [SerializeField] private ObjectElementSkillBehaviour windChildPrefabs;

        private ReactiveProperty<AnimationState.Spell1> FeatureIndexSpell1State { get; } =
            new ReactiveProperty<AnimationState.Spell1>(AnimationState.Spell1.Spell1A);

        private float TimeKnockUpObject => timeKnockUpObject;

        private IDisposable ResetSpellAfterTimerDisposable { get; set; }

        private ObjectPoolSkillBehaviour WindChildEffectPool { get; set; }

        private ObjectElementSkillBehaviour WindChildPrefabs => windChildPrefabs;

        protected override void Awake()
        {
            base.Awake();
            WindChildEffectPool = new ObjectPoolSkillBehaviour(photonView, WindChildPrefabs, transform);
        }

        protected override void Start()
        {
            if (!photonView.IsMine) return;
            base.Start();

            JoystickInputFilterObserver
                .OnSpell1AsObservable()
                .Where(_ => IsCanUse())
                .Subscribe(message =>
                {
                    EnterAnimationSpell1();
                    SyncTransformImmediately.SyncRotationWithDirection(message.Direction);
                    ActiveSkillCurrent(message, 100);
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
                .Select(x => x.OnActiveSkillAsObservable()))
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    if (FeatureIndexSpell1State.Value == AnimationState.Spell1.Spell1C)
                        FeatureIndexSpell1State.Value = AnimationState.Spell1.Spell1A;
                    else
                        HandleNextStateSkill(!receiveDamageables.IsNullOrEmpty());

                    SendOutput();
                });

            SkillBehaviourPassive.OnActiveSkillAsObservable()
                .Subscribe(receiveDamageables =>
                {
                    if (FeatureIndexSpell1State.Value == AnimationState.Spell1.Spell1C)
                    {
                        receiveDamageables.ToList().ForEach(x =>
                        {
                            var knockObj = x.GetComponent<IKnockUpable>();
                            knockObj?.BlowUp(TimeKnockUpObject);
                            photonView.RPC("SpawnTwistChildRpc", RpcTarget.All, x.GetTransform.position);
                        });
                    }

                    if (FeatureIndexSpell1State.Value == AnimationState.Spell1.Spell1C)
                        FeatureIndexSpell1State.Value = AnimationState.Spell1.Spell1A;
                    else
                        HandleNextStateSkill(!receiveDamageables.IsNullOrEmpty());

                    SendOutput();
                });

            HandleSpellDash();
            HandleSpell1Animation();
            HandleOnEnterSpell4();

            FeatureIndexSpell1State
                .Where(x => x == AnimationState.Spell1.Spell1A)
                .Subscribe(_ => { SkillReader.SendNextFirstIndex(); });

            FeatureIndexSpell1State
                .Where(x => x == AnimationState.Spell1.Spell1B)
                .Subscribe(_ =>
                {
                    SkillReader.SendNext(1);
                    ResetSpellAfterTimer(8f);
                });

            FeatureIndexSpell1State
                .Where(x => x == AnimationState.Spell1.Spell1C)
                .Subscribe(_ =>
                {
                    SkillReader.SendNextLastIndex();
                    ResetSpellAfterTimer(10f);
                });
        }

        protected override bool IsCanUse()
        {
            return !AnimationStateChecker.IsInStateSpell3.Value
                   && !AnimationStateChecker.IsInStateSpell4.Value;
        }

        private void HandleAnimationState()
        {
            FeatureIndexSpell1State.Value++;
        }

        private void EnterAnimationSpell1()
        {
            Animator.SetTriggerWithBool(Constant.AnimationPram.Q);
            Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State.Value);
        }

        private void HandleSpell1Animation()
        {
            var spell1Smb = Animator.GetBehaviour<ObservableSpell1Smb>();
            spell1Smb.OnStateExitAsObservable()
                .Subscribe(_ => Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State.Value));
        }

        private void HandleSpellDash()
        {
            var spell1DashSmb = Animator.GetBehaviour<ObservableSpell1DashSmb>();
            spell1DashSmb.OnStateEnterAsObservable()
                .Subscribe(_ => SkillBehaviourPassive.ActiveSkill(new InputMessage()));
        }

        private void HandleNextStateSkill(bool isAnyReceiver)
        {
            if (isAnyReceiver)
                HandleAnimationState();
        }

        private void ResetSpellAfterTimer(float time)
        {
            ResetSpellAfterTimerDisposable?.Dispose();
            ResetSpellAfterTimerDisposable = Observable.Timer(TimeSpan.FromSeconds(time))
                .Subscribe(__ => ResetSpell());
        }

        private void ResetSpell()
        {
            FeatureIndexSpell1State.Value = AnimationState.Spell1.Spell1A;
            InitValue(0);
        }

        private void HandleOnEnterSpell4()
        {
            var spell4Smb = Animator.GetBehaviour<ObservableSpell4Smb>();
            spell4Smb.OnStateEnterAsObservable()
                .Subscribe(_ => ResetSpell());
        }

        [PunRPC]
        private void SpawnTwistChildRpc(Vector3 position)
        {
            WindChildEffectPool.RentAsync().Subscribe(x => { x.transform.position = position; });
        }
    }
}