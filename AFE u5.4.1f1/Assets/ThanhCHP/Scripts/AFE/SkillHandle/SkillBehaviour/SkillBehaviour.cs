using System;
using System.Collections.Generic;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillBehaviour
    {
        void ActiveSkill(IInputMessage inputMessage);
        IObservable<IEnumerable<IReceiveDamageable>> OnActiveSkillAsObservable();
        ISkillOutputMessage GetSkillOutputMessage();
        ISkillOutputMessage GetSkillOutputMessageInit(float timeInit);
    }

    public abstract class SkillBehaviour : MonoBehaviourPun,
        ISkillBehaviour,
        IInitialize<IAnimationStateChecker>,
        IInitialize<IChampionConfig>
    {
        private ISkillConfig skillConfigCache;
        [SerializeField] private SkillModel skillModel;

        protected ISkillConfig SkillConfig =>
            skillConfigCache ?? (skillConfigCache = Instantiate(skillModel));

        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        protected IChampionConfig ChampionConfig { get; private set; }

        protected Subject<IEnumerable<IReceiveDamageable>> ActiveSkillSubject { get; } =
            new Subject<IEnumerable<IReceiveDamageable>>();

        protected SyncTransformImmediately SyncTransformImmediately { get; private set; }

        public void Initialize(IAnimationStateChecker init)
        {
            AnimationStateChecker = init;
        }

        public void Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }

        public virtual void ActiveSkill(IInputMessage inputMessage)
        {
            ActiveSkillSubject.OnNext(null);
        }

        public IObservable<IEnumerable<IReceiveDamageable>> OnActiveSkillAsObservable()
        {
            return ActiveSkillSubject;
        }

        public ISkillOutputMessage GetSkillOutputMessage()
        {
            return new SkillOutputMessage(SkillConfig.IconCurrent, GetCooldown());
        }

        public ISkillOutputMessage GetSkillOutputMessageInit(float timeInit)
        {
            return new SkillOutputMessage(SkillConfig.IconCurrent, timeInit);
        }

        protected float GetCooldown()
        {
            return SkillConfig.Cooldown.Value - SkillConfig.Cooldown.Value * ChampionConfig.CooldownSkillBonus.Value;
        }

        protected virtual void Awake()
        {
            SyncTransformImmediately = gameObject.GetOrAddComponent<SyncTransformImmediately>();
        }

        protected virtual void Start()
        {
        }
    }
}