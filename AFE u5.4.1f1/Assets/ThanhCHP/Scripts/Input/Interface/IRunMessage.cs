using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public interface IRunMessage
    {
        Vector3 Direction { get; }
    }

    public class RunMessage : IRunMessage
    {
        public Vector3 Direction { get; }

        public RunMessage(Vector3 direction)
        {
            Direction = direction;
        }
    }
}