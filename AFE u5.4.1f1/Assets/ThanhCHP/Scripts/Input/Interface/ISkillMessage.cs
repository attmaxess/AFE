namespace Com.Beetsoft.AFE
{
    public interface ISkillMessage
    {
        IReceiveDamageable ObjectReceive { get; }
        float PhysicDamage { get; }
        float MagicDamage { get; }
    }

    public class SkillMessage : ISkillMessage
    {
        public IReceiveDamageable ObjectReceive { get; }
        public float PhysicDamage { get; }
        public float MagicDamage { get; }
    }
}