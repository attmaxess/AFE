using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IInputMessage
    {
        IReceiveDamageable ObjectReceive { get; }
        float PhysicDamage { get; }
        float MagicDamage { get; }
        Vector3 Direction { get; }
    }

    public class InputMessage : IInputMessage
    {
        public IReceiveDamageable ObjectReceive { get; }
        public float PhysicDamage { get; }
        public float MagicDamage { get; }
        public Vector3 Direction { get; }

        public InputMessage(IReceiveDamageable objectReceive, float physicDamage, float magicDamage, Vector3 direction)
        {
            ObjectReceive = objectReceive;
            PhysicDamage = physicDamage;
            MagicDamage = magicDamage;
            Direction = direction;
        }

        public InputMessage(IReceiveDamageable objectReceive, Vector3 direction)
        {
            ObjectReceive = objectReceive;
            Direction = direction;
        }

        public InputMessage(Vector3 direction)
        {
            Direction = direction;
        }

        public InputMessage(IReceiveDamageable objectReceive)
        {
            ObjectReceive = objectReceive;
        }

        public InputMessage()
        {

        }
    }
}