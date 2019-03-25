using System;
using Photon.Pun;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Com.Beetsoft.AFE
{
    public interface ISkillBehaviour
    {
        void ActiveSkill(IInputMessage inputMessage);
        IObservable<Unit> OnActiveSkillAsObservable();
    }

    public abstract class SkillBehaviour : MonoBehaviourPun,
        ISkillBehaviour,
        IInitialize<IAnimationStateChecker>,
        IInitialize<IChampionConfig>
    {
        [SerializeField] private SkillModel skillModel;
        private ISkillConfig skillConfigCache = null;

        protected ISkillConfig SkillConfig =>
            skillConfigCache ?? (skillConfigCache = Object.Instantiate(skillModel));

        public abstract void ActiveSkill(IInputMessage inputMessage);

        public IObservable<Unit> OnActiveSkillAsObservable()
        {
            throw new NotImplementedException();
        }

        protected IAnimationStateChecker AnimationStateChecker { get; private set; }

        public void Initialize(IAnimationStateChecker init)
        {
            AnimationStateChecker = init;
        }
        
        protected IChampionConfig ChampionConfig { get; private set; }

        public void Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }
    }
}