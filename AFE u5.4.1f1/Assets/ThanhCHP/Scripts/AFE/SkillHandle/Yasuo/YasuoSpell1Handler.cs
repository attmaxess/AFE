using System;
using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using UniRx;
using UnityEngine;
using AnimationState = Com.Beetsoft.AFE.Enumerables.AnimationState;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1Handler : SkillHandler, ISkillSpell_1
    {
        [SerializeField] private float timeKnockUpObject = 0.7f;

        private ReactiveProperty<AnimationState.Spell1> FeatureIndexSpell1State { get; } =
            new ReactiveProperty<AnimationState.Spell1>(AnimationState.Spell1.Spell1A);

        private float TimeKnockUpObject => timeKnockUpObject;

        private IDisposable ResetSpellAfterTimerDisposable { get; set; }

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
                        receiveDamageables.ToList().ForEach(x =>
                        {
                            var knockObj = x.GetComponent<IKnockUpable>();
                            knockObj?.BlowUp(TimeKnockUpObject);
                        });

                    if (FeatureIndexSpell1State.Value == AnimationState.Spell1.Spell1C)
                        FeatureIndexSpell1State.Value = AnimationState.Spell1.Spell1A;
                    else
                        HandleNextStateSkill(!receiveDamageables.IsNullOrEmpty());

                    SendOutput();
                });

            HandleSpellDash();

            FeatureIndexSpell1State
                .Where(x => x == AnimationState.Spell1.Spell1A)
                .Subscribe(_ => { SkillReader.SendNextFirstIndex(); });

            FeatureIndexSpell1State
                .Where(x => x == AnimationState.Spell1.Spell1B)
                .Subscribe(_ =>
                {
                    SkillReader.SendNext(1);
                    ResetSpellAfterTimer(6f);
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
    }
}