using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class JoystickInputFilter : MonoBehaviour, IJoystickInputFilter, IJoystickInputFilterObserver
    {
        private Subject<IRunMessage> RunSubject { get; } = new Subject<IRunMessage>();
        private Subject<IRunMessage> IdleSubject { get; } = new Subject<IRunMessage>();
        private Subject<ISkillMessage> BasicAttackSubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell1Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell2Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell3Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell4Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> RecallSubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> DefaultSpellASubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> DefaultSpellBSubject { get; } = new Subject<ISkillMessage>();

        public void Run(IRunMessage message)
        {
            RunSubject.OnNext(message);
        }

        public void Idle(IRunMessage message)
        {
            IdleSubject.OnNext(message);
        }

        public void BasicAttack(ISkillMessage message)
        {
            BasicAttackSubject.OnNext(message);
        }

        public void Spell1(ISkillMessage message)
        {
            Spell1Subject.OnNext(message);
        }

        public void Spell2(ISkillMessage message)
        {
            Spell2Subject.OnNext(message);
        }

        public void Spell3(ISkillMessage message)
        {
            Spell3Subject.OnNext(message);
        }

        public void Spell4(ISkillMessage message)
        {
            Spell4Subject.OnNext(message);
        }

        public void Recall(ISkillMessage message)
        {
            RecallSubject.OnNext(message);
        }

        public void DefaultSpellA(ISkillMessage message)
        {
            DefaultSpellASubject.OnNext(message);
        }

        public void DefaultSpellB(ISkillMessage message)
        {
            DefaultSpellBSubject.OnNext(message);
        }

        public IObservable<IRunMessage> OnRunAsObservable()
        {
            return RunSubject;
        }

        public IObservable<IRunMessage> OnIdleAsObservable()
        {
            return IdleSubject;
        }

        public IObservable<ISkillMessage> OnBasicAttackAsObservable()
        {
            return BasicAttackSubject;
        }

        public IObservable<ISkillMessage> OnSpell1AsObservable()
        {
            return Spell1Subject;
        }

        public IObservable<ISkillMessage> OnSpell2AsObservable()
        {
            return Spell2Subject;
        }

        public IObservable<ISkillMessage> OnSpell3AsObservable()
        {
            return Spell3Subject;
        }

        public IObservable<ISkillMessage> OnSpell4AsObservable()
        {
            return Spell4Subject;
        }

        public IObservable<ISkillMessage> OnRecallAsObservable()
        {
            return RecallSubject;
        }

        public IObservable<ISkillMessage> OnDefaultSpellAAsObservable()
        {
            return DefaultSpellASubject;
        }

        public IObservable<ISkillMessage> OnDefaultSpellBAsObservable()
        {
            return DefaultSpellBSubject;
        }
    }
}