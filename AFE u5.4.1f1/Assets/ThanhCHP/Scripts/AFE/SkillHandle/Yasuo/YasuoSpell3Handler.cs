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
        protected override void Start()
        {
            base.Start();
            JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .Subscribe(message =>
                {
                    var skillBehavior = SkillReader.GetSkillBehaviourCurrent();
                    skillBehavior.ActiveSkill(message);
                });

            foreach (var onActiveSkill in SkillReader.SkillBehaviours.Distinct()
                .Select(x => x.OnActiveSkillAsObservable()))
                onActiveSkill.Subscribe(receiveDamageables =>
                {
                    Debug.Log(receiveDamageables.IsNullOrEmpty());
                    if (receiveDamageables.IsNullOrEmpty()) return;
                    var direction = (receiveDamageables.First().GetTransform.position - transform.position).normalized;
                    ChampionTransform.Forward = direction;
                    SendOutput();
                    Animator.SetTriggerWithBool(Constant.AnimationPram.E);
                });
        }
    }
}