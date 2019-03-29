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
        private AnimationState.Spell1 FeatureIndexSpell1State { get; set; } = AnimationState.Spell1.Spell1A;
        
        protected override void Start()
        {
            base.Start();
            SkillReader.SendNextFirstIndex();

            JoystickInputFilterObserver
                .OnSpell1AsObservable()
                .Where(_ => !AnimationStateChecker.IsInStateSpell3.Value)
                .Subscribe(message =>
                {
                    Animator.SetTriggerWithBool(Constant.AnimationPram.Q);
                    SyncTransformImmediately.SyncRotationWithDirection(message.Direction);
                    ActiveSkillCurrent(message, 100);
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
                .Select(x => x.OnActiveSkillAsObservable()))
            {
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    if (receiveDamageables.IsNullOrEmpty())
                    {
                        SendOutput();
                    }
                    else
                    {
                        SkillReader.SendNext();
                        HandleAnimationState();
                        SendOutput();
                    }
                });
            }
            
            //HandleSpellDash();
        }

        private void HandleAnimationState()
        {
            FeatureIndexSpell1State++;
            if ((int) FeatureIndexSpell1State == Constant.Yasuo.QClipAmount)
            {
                FeatureIndexSpell1State = AnimationState.Spell1.Spell1A;
                SkillReader.SendNextFirstIndex();
            }

            Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State);
            Animator.SetBool(Constant.AnimationPram.IdleBool, true);
        }

        private void HandleSpellDash()
        {
            JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .SelectMany(_ =>
                    JoystickInputFilterObserver.OnSpell1AsObservable().TakeUntil(
                        Observable.Timer(TimeSpan.FromSeconds(Constant.Yasuo.OffsetTimeSpell3AndSpell1))))
                .Subscribe(_ =>
                {
                    Animator.SetInteger(Constant.AnimationPram.QInt, (int) AnimationState.Spell1.Spell1_Dash);
                    Animator.SetBool(Constant.AnimationPram.IdleBool, false);
                });
        }
    }
}