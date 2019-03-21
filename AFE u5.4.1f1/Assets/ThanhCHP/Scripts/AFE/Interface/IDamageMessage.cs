namespace Com.Beetsoft.AFE
{
    public interface IDamageMessage
    {
        float PhysicDamage { get; }
        float MagicDamage { get; }
    }

    public class DamageMessage : IDamageMessage
    {
        public float PhysicDamage { get; }
        public float MagicDamage { get; }

        public DamageMessage()
        {
        }

        public DamageMessage(float physicDamage, float magicDamage)
        {
            PhysicDamage = physicDamage;
            MagicDamage = magicDamage;
        }
    }
}