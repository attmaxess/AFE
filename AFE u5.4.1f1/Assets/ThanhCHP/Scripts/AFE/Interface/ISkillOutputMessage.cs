using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillOutputMessage
    {
        Sprite Icon { get; }
        float Cooldown { get; }
    }

    public class SkillOutputMessage : ISkillOutputMessage
    {
        public Sprite Icon { get; }
        public float Cooldown { get; }

        public SkillOutputMessage(Sprite icon, float cooldown)
        {
            Icon = icon;
            Cooldown = cooldown;
        }
    }
}