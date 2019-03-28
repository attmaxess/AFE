using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

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
        public SkillBehaviour[] SkillBehaviours { get; }

        private int SkillIndex { get; set; } = -1;

        private ReactiveProperty<ISkillBehaviour> SkillBehaviourCurrent { get; } = new ReactiveProperty<ISkillBehaviour>();

        public IObservable<ISkillBehaviour> OnChangeSkillBehaviourAsObservable() => SkillBehaviourCurrent;

        public void Initialize()
        {
            SendNext();
        }

        public void SendNext()
        {
            SkillIndex = Mathf.Min(SkillBehaviours.Length - 1, ++SkillIndex);
            SkillBehaviourCurrent.Value = SkillBehaviours[SkillIndex];
        }

        public void SendNextLastIndex()
        {
            SkillIndex = SkillBehaviours.Length - 1;
            SkillBehaviourCurrent.Value = SkillBehaviours.Last();
        }

        public void SendNextFirstIndex()
        {
            SkillIndex = 0;
            SkillBehaviourCurrent.Value = SkillBehaviours.First();
        }

        public ISkillBehaviour GetSkillBehaviourCurrent()
        {
            return SkillBehaviourCurrent.Value;
        }

        public SkillReader()
        {
        }

        public SkillReader(SkillBehaviour[] skillBehaviours, int skillIndex)
        {
            SkillBehaviours = skillBehaviours;
            SkillIndex = skillIndex;
            SkillBehaviourCurrent.Value = skillBehaviours[skillIndex];
        }
    }
}