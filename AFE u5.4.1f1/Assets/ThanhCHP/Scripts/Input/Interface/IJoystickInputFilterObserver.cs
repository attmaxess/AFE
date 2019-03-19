using System;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IJoystickInputFilterObserver
    {
        IObservable<Vector3> OnRunAsObservable();
        IObservable<Vector3> OnIdleAsObservable();
        IObservable<GameObject> OnBasicAttackAsObservable();
        IObservable<Vector3> OnSpell1AsObservable();
        IObservable<Vector3> OnSpell2AsObservable();
        IObservable<Vector3> OnSpell3AsObservable();
        IObservable<Vector3> OnSpell4AsObservable();
        IObservable<Vector3> OnRecallAsObservable();
        IObservable<Vector3> OnDefaultSpellAAsObservable();
        IObservable<Vector3> OnDefaultSpellBAsObservable();
    }
}