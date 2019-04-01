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
        private AnimationState.Spell1 FeatureIndexSpell1State { get; set; } = AnimationState.Spell1.Spell1A;

        private float TimeKnockUpObject => timeKnockUpObject;

        protected override void Start()
        {
            if(!photonView.IsMine) return;
            base.Start();

            JoystickInputFilterObserver
                .OnSpell1AsObservable()
                .Where(_ => !AnimationStateChecker.IsInStateSpell3.Value)
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
                    HandleNextStateSkill(!receiveDamageables.IsNullOrEmpty());
                });

            SkillBehaviourPassive.OnActiveSkillAsObservable()
                .Subscribe(receiveDamageables =>
                {               
                    if (FeatureIndexSpell1State == AnimationState.Spell1.Spell1C)
                    {
                        receiveDamageables.ToList().ForEach(x =>
                        {
                            var knockObj = x.GetComponent<IKnockUpable>();
                            knockObj?.BlowUp(TimeKnockUpObject);
                        });
                    }
                    
                    HandleNextStateSkill(!receiveDamageables.IsNullOrEmpty());
                });

            HandleSpellDash();
        }

        private void HandleAnimationState()
        {
            FeatureIndexSpell1State++;
            if ((int) FeatureIndexSpell1State == Constant.Yasuo.QClipAmount)
            {
                FeatureIndexSpell1State = AnimationState.Spell1.Spell1A;
                SkillReader.SendNextFirstIndex();
            }

            Animator.SetBool(Constant.AnimationPram.IdleBool, true);
        }

        private void EnterAnimationSpell1()
        {
            Animator.SetTriggerWithBool(Constant.AnimationPram.Q);
            Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State);
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
            {
                SkillReader.SendNext();
                HandleAnimationState();
                SendOutput();
            }
            else
            {
                SendOutput();
            }
        }
    }
}