using System.Linq;
using AFE.Extensions;
using ExtraLinq;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell3Handler : SkillHandler, ISkillSpell_3
    {
        private void Start()
        {
            JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .Subscribe(message =>
                {
                    var skillBehavior = SkillReader.GetSkillBehaviourCurrent();
                    skillBehavior.ActiveSkill(message);
                    SkillMessageOutputReactiveProperty.Value = skillBehavior.GetSkillOutputMessage();
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
                .Select(x => x.OnActiveSkillAsObservable()))
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    Debug.Log(receiveDamageables.IsNullOrEmpty());
                    if (receiveDamageables.IsNullOrEmpty()) return;
                    
                    Animator.SetTriggerWithBool(Constant.AnimationPram.E);
                });
        }
    }
}