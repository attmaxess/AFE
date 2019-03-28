using AFE.Extensions;
using ExtraLinq;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell3Handler : SkillHandler, ISkillSpell_3
    {
        private void Start()
        {
            this.JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.E))
                .Subscribe(message =>
                {
                    var skillBehavior = SkillReader.GetSkillBehaviourCurrent();
                    skillBehavior.ActiveSkill(message);
                    SkillMessageOutputReactiveProperty.Value = skillBehavior.GetSkillOutputMessage();
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
             .Select(x => x.OnActiveSkillAsObservable()))
            {
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    Debug.Log(receiveDamageables.IsNullOrEmpty());
                    if (receiveDamageables.IsNullOrEmpty())
                    {
                        return;
                    }

                    // Move to target + attack

                });
            }
        }
    }
}