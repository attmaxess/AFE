using AFE.Extensions;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell3Handler : SkillHandler
    {
        private void Start()
        {
            this.JoystickInputFilterObserver
                .OnSpell3AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.E))
                .Subscribe();
        }
    }
}