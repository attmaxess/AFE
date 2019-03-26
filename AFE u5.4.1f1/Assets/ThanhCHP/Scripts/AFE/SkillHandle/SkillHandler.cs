using System;
using Photon.Pun;
using UniRx;
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

        [SerializeField] private SkillReader skillReader;
        protected IJoystickInputFilterObserver JoystickInputFilterObserver { get; private set; }

        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        protected IChampionConfig ChampionConfig { get; private set; }

        protected ISkillConfig SkillConfig => skillConfig;

        protected Animator Animator { get; private set; }

        protected ReactiveProperty<ISkillOutputMessage> SkillMessageOutputReactiveProperty { get; } = new ReactiveProperty<ISkillOutputMessage>();

        protected SkillReader SkillReader => skillReader;

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

        public ISkillOutputMessage SkillMessageOutputCurrent()
        {
            return SkillMessageOutputReactiveProperty.Value;
        }

        public IObservable<ISkillOutputMessage> OnReceiveSkillMessageOutputAsObservable()
        {
            return SkillMessageOutputReactiveProperty;
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

        protected float GetCooldownSkill()
        {
            return ChampionConfig.CooldownSkillBonus.Value * SkillConfig.Cooldown.Value;
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}