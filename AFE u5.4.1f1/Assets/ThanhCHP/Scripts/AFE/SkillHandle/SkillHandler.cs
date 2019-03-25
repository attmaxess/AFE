using System;
using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public abstract class SkillHandler : MonoBehaviourPun,
        ISkillHandler,
        IInitialize<IJoystickInputFilterObserver>,
        IInitialize<IAnimationStateChecker>,
        IInitialize<IChampionConfig>
    {
        [SerializeField] private SkillModel skillConfig;
        protected IJoystickInputFilterObserver JoystickInputFilterObserver { get; private set; }

        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        protected IChampionConfig ChampionConfig { get; private set; }

        protected ISkillConfig SkillConfig => skillConfig;

        protected Animator Animator { get; private set; }

        void IInitialize<IAnimationStateChecker>.Initialize(IAnimationStateChecker init)
        {
            AnimationStateChecker = init;
        }

        void IInitialize<IChampionConfig>.Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }

        void IInitialize<IJoystickInputFilterObserver>.Initialize(IJoystickInputFilterObserver init)
        {
            JoystickInputFilterObserver = init;
        }

        protected float GetPhysicDamageCurrent()
        {
            return SkillConfig.PhysicDamage.Value + GetPhysicDamageBonus();
        }

        protected float GetPhysicDamageBonus()
        {
            return ChampionConfig.AttackDamage.Value * SkillConfig.PhysicDamageBonus;
        }

        protected float GetMagicDamageCurrent()
        {
            return SkillConfig.MagicDamage.Value + GetMagicDamageBonus();
        }

        protected float GetMagicDamageBonus()
        {
            return ChampionConfig.AbilityPower.Value * SkillConfig.MagicDamageBonus;
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public ISkillOutputMessage SkillMessageOutputCurrent()
        {
            throw new NotImplementedException();
        }

        public IObservable<ISkillOutputMessage> OnReceiveSkillMessageOutputAsObservable()
        {
            throw new NotImplementedException();
        }
    }
}