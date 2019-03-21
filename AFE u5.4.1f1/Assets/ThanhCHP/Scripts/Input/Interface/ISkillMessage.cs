using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillMessage
    {
    }

    public class SkillMessage : ISkillMessage
    {

        public SkillMessage()
        {
        }

        public SkillMessage(Vector3 dir)
        {
            this.dir = dir;
        }

        public Vector3 dir { get; }
    }
}