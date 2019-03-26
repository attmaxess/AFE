using System;
using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using UnityEngine;
using UniRx;
using AnimationState = Com.Beetsoft.AFE.Enumerables.AnimationState;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1Handler : SkillHandler
    {
        private AnimationState.Spell1 FeatureIndexSpell1State { get; set; } = AnimationState.Spell1.Spell1A;

        private void Start()
        {
            SkillReader.SendNext();

            this.JoystickInputFilterObserver
                .OnSpell1AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.Q))
                .Subscribe(message =>
                {
                    var skillBehaviour = SkillReader.GetSkillBehaviourCurrent();
                    skillBehaviour.ActiveSkill(message);
                    SkillMessageOutputReactiveProperty.Value = skillBehaviour.GetSkillOutputMessage();
                    SkillReader.SendNext();
                });

            this.JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .SelectMany(_ =>
                    this.JoystickInputFilterObserver.OnSpell1AsObservable().TakeUntil(
                        Observable.Timer(TimeSpan.FromMilliseconds(Constant.Yasuo.OffsetTimeSpell3AndSpell1))))
                .Subscribe(_ =>
                {
                    Animator.SetInteger(Constant.AnimationPram.QInt, (int) AnimationState.Spell1.Spell1_Dash);
                    Animator.SetBool(Constant.AnimationPram.IdleBool, false);
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours
                .Select(x => x.OnActiveSkillAsObservable()))
            {
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    if (receiveDamageables.IsNullOrEmpty()) return;
                    SkillReader.SendNext();
                    HandleAnimationState();
                });
            }
        }

        private void HandleAnimationState()
        {
            FeatureIndexSpell1State++;
            if ((int) FeatureIndexSpell1State == Constant.Yasuo.QClipAmount)
                FeatureIndexSpell1State = AnimationState.Spell1.Spell1A;
            Animator.SetInteger(Constant.AnimationPram.QInt, (int) FeatureIndexSpell1State);
            Animator.SetBool(Constant.AnimationPram.IdleBool, true);
        }
    }
}