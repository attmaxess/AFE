using AFE.Extensions;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell4Handler : SkillHandler    ,ISkillSpell_4
    {
        protected override void Start()
        {
            base.Start();
            this.JoystickInputFilterObserver
                .OnSpell4AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.R))
                .Subscribe();
        }
    }
}