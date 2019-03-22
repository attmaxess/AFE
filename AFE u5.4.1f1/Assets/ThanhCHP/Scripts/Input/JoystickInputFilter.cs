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
        private Subject<ISkillMessage> BasicAttackSubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell1Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell2Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell3Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> Spell4Subject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> RecallSubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> DefaultSpellASubject { get; } = new Subject<ISkillMessage>();
        private Subject<ISkillMessage> DefaultSpellBSubject { get; } = new Subject<ISkillMessage>();

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

        public void BasicAttack(ISkillMessage message)
        {
            var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig);

            var mes = new SkillMessage(ireceiver,
                ChampionConfig.AttackDamage.Value,
                ChampionConfig.AttackDamagePerLevel,
                message.Direction);
            BasicAttackSubject.OnNext(mes);
        }

        public void Spell1(ISkillMessage message)
        {
            var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig);
            var mes = new SkillMessage(ireceiver,
                ChampionConfig.AttackDamage.Value,
                ChampionConfig.AttackDamagePerLevel,
                message.Direction);
            Spell1Subject.OnNext(mes);
        }

        public void Spell2(ISkillMessage message)
        {
            var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig);
            var mes = new SkillMessage(ireceiver,
                ChampionConfig.AttackDamage.Value,
                ChampionConfig.AttackDamagePerLevel,
                message.Direction);
            Spell2Subject.OnNext(mes);
        }

        public void Spell3(ISkillMessage message)
        {
            var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig);
            var mes = new SkillMessage(ireceiver,
                ChampionConfig.AttackDamage.Value,
                ChampionConfig.AttackDamagePerLevel,
                message.Direction);
            Spell3Subject.OnNext(mes);
        }

        public void Spell4(ISkillMessage message)
        {
            var ireceiver = gameObject.ReceiverDamageNearest(ChampionConfig);
            var mes = new SkillMessage(ireceiver,
                ChampionConfig.AttackDamage.Value,
                ChampionConfig.AttackDamagePerLevel,
                message.Direction);
            Spell4Subject.OnNext(mes);
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