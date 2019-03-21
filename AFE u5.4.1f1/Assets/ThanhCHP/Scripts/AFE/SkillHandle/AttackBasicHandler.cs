using System;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public abstract class AttackBasicHandler : SkillHandler
    {
        protected virtual void Start()
        {
            if (!photonView.IsMine) return;

            var applySkillTimer =
                Observable.Timer(TimeSpan.FromMilliseconds(Constant.Yasuo.TimeDelayApplyDamageAttackBasicMilliseconds));

            JoystickInputFilterObserver.OnBasicAttackAsObservable()
                .RequestApplySkill(applySkillTimer, AnimationStateChecker.IsBasicAttack.Where(x => !x))
                .RepeatUntilDestroy(this)
                .Subscribe(ApplyDamage);
        }

        protected abstract void ApplyDamage(ISkillMessage message);
    }
}