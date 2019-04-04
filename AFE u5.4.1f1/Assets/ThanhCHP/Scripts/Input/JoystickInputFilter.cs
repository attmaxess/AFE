using Photon.Pun;
using System;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class JoystickInputFilter : MonoBehaviourPun,
        IJoystickInputFilter,
        IJoystickInputFilterObserver,
        IInitialize<IChampionConfig>
    {
        private Subject<IRunMessage> RunSubject { get; } = new Subject<IRunMessage>();
        private Subject<IRunMessage> IdleSubject { get; } = new Subject<IRunMessage>();
        private Subject<IInputMessage> BasicAttackSubject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> Spell1Subject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> Spell2Subject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> Spell3Subject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> Spell4Subject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> RecallSubject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> DefaultSpellASubject { get; } = new Subject<IInputMessage>();
        private Subject<IInputMessage> DefaultSpellBSubject { get; } = new Subject<IInputMessage>();

        private IChampionConfig ChampionConfig { get; set; }

        void IInitialize<IChampionConfig>.Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }

        public void Run(IRunMessage message)
        {
            RunSubject.OnNext(message);                                
        }

        public void Idle(IRunMessage message)
        {
            IdleSubject.OnNext(message);
        }

        public void BasicAttack(IInputMessage message)
        {
            /* var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig.Range.Value);

             var mes = new InputMessage(ireceiver,
                 ChampionConfig.AttackDamage.Value,
                 ChampionConfig.AttackDamagePerLevel,
                 message.Direction);   */
            BasicAttackSubject.OnNext(message);
        }

        public void Spell1(IInputMessage message)
        {
            Spell1Subject.OnNext(message);
        }

        public void Spell2(IInputMessage message)
        {
            Spell2Subject.OnNext(message);
        }

        public void Spell3(IInputMessage message)
        {
            Spell3Subject.OnNext(message);
        }

        public void Spell4(IInputMessage message)
        {
            Spell4Subject.OnNext(message);
        }

        public void Recall(IInputMessage message)
        {
            RecallSubject.OnNext(message);
        }

        public void DefaultSpellA(IInputMessage message)
        {
            DefaultSpellASubject.OnNext(message);
        }

        public void DefaultSpellB(IInputMessage message)
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

        public IObservable<IInputMessage> OnBasicAttackAsObservable()
        {
            return BasicAttackSubject;
        }

        public IObservable<IInputMessage> OnSpell1AsObservable()
        {
            return Spell1Subject;
        }

        public IObservable<IInputMessage> OnSpell2AsObservable()
        {
            return Spell2Subject;
        }

        public IObservable<IInputMessage> OnSpell3AsObservable()
        {
            return Spell3Subject;
        }

        public IObservable<IInputMessage> OnSpell4AsObservable()
        {
            return Spell4Subject;
        }

        public IObservable<IInputMessage> OnRecallAsObservable()
        {
            return RecallSubject;
        }

        public IObservable<IInputMessage> OnDefaultSpellAAsObservable()
        {
            return DefaultSpellASubject;
        }

        public IObservable<IInputMessage> OnDefaultSpellBAsObservable()
        {
            return DefaultSpellBSubject;
        }


    }
}