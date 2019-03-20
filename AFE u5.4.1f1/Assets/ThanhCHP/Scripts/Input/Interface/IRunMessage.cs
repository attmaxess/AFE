using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IRunMessage
    {
        Vector3 Direction { get; }
        Vector3 Rotation { get; }
    }

    public class RunMessage : IRunMessage
    {
        public Vector3 Direction { get; }

        public Vector3 Rotation { get; }

        public RunMessage(Vector3 direction, Vector3 rotation)
        {
            Direction = direction;
            Rotation = rotation;
        }
    }
}