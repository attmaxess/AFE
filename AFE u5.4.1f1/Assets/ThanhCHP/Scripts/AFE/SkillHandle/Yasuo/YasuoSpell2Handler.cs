using AFE.Extensions;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell2Handler : SkillHandler
    {
        private void Start()
        {
            this.JoystickInputFilterObserver
                .OnSpell2AsObservable()
                .Do(_ => Animator.SetTriggerWithBool(Constant.AnimationPram.W))
                .Subscribe();
        }
    }
}