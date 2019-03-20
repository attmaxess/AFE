using System;
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
}