using AFE.Extensions;
using ExtraLinq;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell2Handler : SkillHandler, ISkillSpell_2
    {
        private void Start()
        {
            this.SkillReader.SendNextFirstIndex();

            this.JoystickInputFilterObserver
                .OnSpell2AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.W))
                .Subscribe(message =>
                {
                    var skillBehaviour = SkillReader.GetSkillBehaviourCurrent();
                    skillBehaviour.ActiveSkill(message);
                    SkillMessageOutputReactiveProperty.Value = skillBehaviour.GetSkillOutputMessage();
                });


            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
               .Select(x => x.OnActiveSkillAsObservable()))
            {
                onActiveSkill.Subscribe(receiveDamageables =>
                {

                });
            }
        }
    }
}