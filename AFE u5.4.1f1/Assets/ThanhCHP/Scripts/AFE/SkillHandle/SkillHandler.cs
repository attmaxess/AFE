using System;
using ExtraLinq;
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
        [SerializeField] private SkillBehaviour skillBehaviourPassive;
        [SerializeField] private SkillBehaviour[] skillBehaviours;
        [SerializeField] private SkillModel skillConfig;

        private ISkillBehaviour[] SkillBehaviours => skillBehaviours;

        protected ISkillBehaviour SkillBehaviourPassive => skillBehaviourPassive;

        protected IJoystickInputFilterObserver JoystickInputFilterObserver { get; private set; }

        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        protected IChampionConfig ChampionConfig { get; private set; }

        protected ISkillConfig SkillConfig => skillConfig;

        protected Animator Animator { get; private set; }

        private ReactiveProperty<ISkillOutputMessage> SkillMessageOutputReactiveProperty { get; } =
            new ReactiveProperty<ISkillOutputMessage>();

        protected SkillReader SkillReader { get; private set; }

        protected SyncTransformImmediately SyncTransformImmediately { get; private set; }

        protected IChampionTransform ChampionTransform { get; private set; }

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

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            SkillReader = !SkillBehaviours.IsNullOrEmpty() ? new SkillReader(SkillBehaviours, 0) : new SkillReader();
            SyncTransformImmediately = gameObject.GetOrAddComponent<SyncTransformImmediately>();
            ChampionTransform = GetComponent<IChampionTransform>();
        }

        protected virtual void Start()
        {
            InitValue(0.5f);
        }

        protected virtual void ActiveSkillCurrent(IInputMessage message, int millisecondDelay)
        {
            var skillBehaviour = SkillReader.GetSkillBehaviourCurrent();
            Observable.Timer(TimeSpan.FromMilliseconds(millisecondDelay))
                .Subscribe(_ => skillBehaviour.ActiveSkill(message));
        }

        protected void InitValue(float timeInit)
        {
            SkillMessageOutputReactiveProperty.Value =
                SkillReader.GetSkillBehaviourCurrent().GetSkillOutputMessageInit(timeInit);
        }

        protected void SendOutput()
        {
            SkillMessageOutputReactiveProperty.Value = SkillReader.GetSkillBehaviourCurrent().GetSkillOutputMessage();
        }

        protected abstract bool IsCanUse();
    }
}