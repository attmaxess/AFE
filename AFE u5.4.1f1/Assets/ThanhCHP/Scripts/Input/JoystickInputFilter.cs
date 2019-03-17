using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class JoystickInputFilter : MonoBehaviour, IJoystickInputFilter, IJoystickInputFilterObserver
    {
        private Subject<Vector3> RunSubject { get; } = new Subject<Vector3>();
        private Subject<Vector3> IdleSubject { get; } = new Subject<Vector3>();
        private Subject<GameObject> BasicAttackSubject { get; } = new Subject<GameObject>();
        private Subject<Vector3> Spell1Subject { get; } = new Subject<Vector3>();
        private Subject<Vector3> Spell2Subject { get; } = new Subject<Vector3>();
        private Subject<Vector3> Spell3Subject { get; } = new Subject<Vector3>();
        private Subject<Vector3> Spell4Subject { get; } = new Subject<Vector3>();
        private Subject<Vector3> RecallSubject { get; } = new Subject<Vector3>();
        private Subject<Vector3> DefaultSpellASubject { get; } = new Subject<Vector3>();
        private Subject<Vector3> DefaultSpellBSubject { get; } = new Subject<Vector3>();

        public void Run(Vector3 dir)
        {
            RunSubject.OnNext(dir);
        }

        public void Idle(Vector3 position)
        {
            IdleSubject.OnNext(position);
        }

        public void BasicAttack(GameObject target)
        {
            BasicAttackSubject.OnNext(target);
        }

        public void Spell1(Vector3 dir)
        {
            Spell1Subject.OnNext(dir);
        }

        public void Spell2(Vector3 dir)
        {
            Spell2Subject.OnNext(dir);
        }

        public void Spell3(Vector3 dir)
        {
            Spell3Subject.OnNext(dir);
        }

        public void Spell4(Vector3 dir)
        {
            Spell4Subject.OnNext(dir);
        }

        public void Recall(Vector3 dir)
        {
            RecallSubject.OnNext(dir);
        }

        public void DefaultSpellA(Vector3 dir)
        {
            DefaultSpellASubject.OnNext(dir);
        }

        public void DefaultSpellB(Vector3 dir)
        {
            DefaultSpellBSubject.OnNext(dir);
        }

        public IObservable<Vector3> OnRunAsObservable()
        {
            return RunSubject;
        }

        public IObservable<Vector3> OnIdleAsObservable()
        {
            return IdleSubject;
        }

        public IObservable<GameObject> OnBasicAttackAsObservable()
        {
            return BasicAttackSubject;
        }

        public IObservable<Vector3> OnSpell1AsObservable()
        {
            return Spell1Subject;
        }

        public IObservable<Vector3> OnSpell2AsObservable()
        {
            return Spell2Subject;
        }

        public IObservable<Vector3> OnSpell3AsObservable()
        {
            return Spell3Subject;
        }

        public IObservable<Vector3> OnSpell4AsObservable()
        {
            return Spell4Subject;
        }

        public IObservable<Vector3> OnRecallAsObservable()
        {
            return RecallSubject;
        }

        public IObservable<Vector3> OnDefaultSpellAAsObservable()
        {
            return DefaultSpellASubject;
        }

        public IObservable<Vector3> OnDefaultSpellBAsObservable()
        {
            return DefaultSpellBSubject;
        }
    }
}