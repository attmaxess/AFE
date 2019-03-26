using AFE.Extensions;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell4Handler : SkillHandler
    {
        private void Start()
        {
            this.JoystickInputFilterObserver
                .OnSpell4AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.R))
                .Subscribe();
        }
    }
}