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
        IObservable<IInputMessage> OnBasicAttackAsObservable();
        IObservable<IInputMessage> OnSpell1AsObservable();
        IObservable<IInputMessage> OnSpell2AsObservable();
        IObservable<IInputMessage> OnSpell3AsObservable();
        IObservable<IInputMessage> OnSpell4AsObservable();
        IObservable<IInputMessage> OnRecallAsObservable();
        IObservable<IInputMessage> OnDefaultSpellAAsObservable();
        IObservable<IInputMessage> OnDefaultSpellBAsObservable();
    }

    public static class JoystickInputFilterObserverExtension
    {
        public static IObservable<IInputMessage> RequestApplySkill<T1, T2>(this IObservable<IInputMessage> request,
            [NotNull] IObservable<T1> applyAfter, [NotNull] IObservable<T2> requestCancer)
        {
            return request.SelectMany(message => applyAfter.Select(_ => message).TakeUntil(requestCancer));
        }
    }
}