using System;
using System.Collections.Generic;
using System.Linq;
using ExtraLinq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Com.Beetsoft.AFE
{
    public interface ISkillReader
    {
        void SendNext();
    }

//    public static class SkillReaderExtension
//    {
//        public static ISkillReaderOutput ReadSkill(this ISkillReader skillReader, ISkillConfig skillConfig)
//        {
//            return new SkillReaderOutput(skillConfig.IconCurrent, skillConfig.);
//        }
//    }

    [Serializable]
    public class SkillReader
    {
        [SerializeField] private List<SkillBehaviour> skillBehaviours;

        private List<SkillBehaviour> SkillBehaviours => skillBehaviours;

        private int SkillIndex { get; set; } = 0;

        public void SendNext()
        {
            SkillIndex++;
            SkillBehaviourCurrent.Value = skillBehaviours[SkillIndex];
        }

        public void SendNextLastIndex()
        {
            SkillIndex = SkillBehaviours.Count - 1;
            SkillBehaviourCurrent.Value = SkillBehaviours.Last();
        }

        public void SendNextFirstIndex()
        {
            SkillIndex = 0;
            SkillBehaviourCurrent.Value = SkillBehaviours.First();
        }

        private ReactiveProperty<ISkillBehaviour> SkillBehaviourCurrent { get; } =
            new ReactiveProperty<ISkillBehaviour>();

        public ISkillBehaviour GetSkillBehaviourCurrent() => SkillBehaviourCurrent.Value;

        public IObservable<ISkillBehaviour> OnChangeSkillBehaviourAsObservable => SkillBehaviourCurrent;
    }
}