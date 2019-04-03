using AFE.Extensions;
using ExtraLinq;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell2Handler : SkillHandler, ISkillSpell_2
    {
        protected override void Start()
        {
            base.Start();
            this.SkillReader.SendNextFirstIndex();

            this.JoystickInputFilterObserver
                .OnSpell2AsObservable()
                .Where(_ => IsCanUse())
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.W))
                .Subscribe(message =>
                {
                    ChampionTransform.Forward = message.Direction;
                    ActiveSkillCurrent(message, 250);
                    SendOutput();
                });


//            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
//               .Select(x => x.OnActiveSkillAsObservable()))
//            {
//                onActiveSkill.Subscribe(receiveDamageables =>
//                {
//
//                });
//            }
        }

        protected override bool IsCanUse()
        {
            return !AnimationStateChecker.IsInStateSpell3.Value
                   && !AnimationStateChecker.IsInStateSpell4.Value
                   && !AnimationStateChecker.IsInStateSpell1.Value;
        }
    }
}