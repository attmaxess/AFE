using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillReaderOutput
    {
        Sprite Icon { get; }
        float Cost { get; }
        float Range { get; }
        float PhysicDamage { get; }
        float MagicDamage { get; }
        float Cooldown { get; }
    }

    public class SkillReaderOutput : ISkillReaderOutput
    {
        public Sprite Icon { get; }
        public float Cost { get; }
        public float Range { get; }
        public float PhysicDamage { get; }
        public float MagicDamage { get; }
        public float Cooldown { get; }
    }
}