using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IJoystickInputFilterObserver
    {
        IObservable<IRunMessage> OnRunAsObservable();
        IObservable<IRunMessage> OnIdleAsObservable();
        IObservable<ISkillMessage> OnBasicAttackAsObservable();
        IObservable<ISkillMessage> OnSpell1AsObservable();
        IObservable<ISkillMessage> OnSpell2AsObservable();
        IObservable<ISkillMessage> OnSpell3AsObservable();
        IObservable<ISkillMessage> OnSpell4AsObservable();
        IObservable<ISkillMessage> OnRecallAsObservable();
        IObservable<ISkillMessage> OnDefaultSpellAAsObservable();
        IObservable<ISkillMessage> OnDefaultSpellBAsObservable();
    }

    public static class JoystickInputFilterObserverExtension
    {
        public static IObservable<ISkillMessage> RequestApplySkill<T1, T2>(this IObservable<ISkillMessage> request,
            [NotNull] IObservable<T1> applyAfter, [NotNull] IObservable<T2> requestCancer)
        {
            return request.SelectMany(message => applyAfter.Select(_ => message))
                .TakeUntil(requestCancer);
        }
    }
}