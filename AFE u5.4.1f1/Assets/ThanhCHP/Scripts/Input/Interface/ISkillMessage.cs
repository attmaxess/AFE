using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface ISkillMessage
    {
        IReceiveDamageable ObjectReceive { get; }
        float PhysicDamage { get; }
        float MagicDamage { get; }
        Vector3 Direction { get; }
    }

    public class SkillMessage : ISkillMessage
    {
        public IReceiveDamageable ObjectReceive { get; }
        public float PhysicDamage { get; }
        public float MagicDamage { get; }
        public Vector3 Direction { get; }

        public SkillMessage(IReceiveDamageable objectReceive, float physicDamage, float magicDamage, Vector3 direction)
        {
            ObjectReceive = objectReceive;
            PhysicDamage = physicDamage;
            MagicDamage = magicDamage;
            Direction = direction;
        }

        public SkillMessage(Vector3 direction)
        {
            Direction = direction;
        }

        public SkillMessage()
        {

        }
    }
}