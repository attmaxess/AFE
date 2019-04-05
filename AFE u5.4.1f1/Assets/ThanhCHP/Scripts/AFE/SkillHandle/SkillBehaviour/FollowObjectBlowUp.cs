using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class FollowObjectBlowUp : MonoBehaviourPun
    {
        private HashSet<IReceiveDamageable> ObjectsBlowUp { get; } = new HashSet<IReceiveDamageable>();

        public IEnumerable<IReceiveDamageable> GetObjectsBlowUp()
        {
            return ObjectsBlowUp;
        }

        public IEnumerable<IReceiveDamageable> GetObjectsBlowUpOrderByHealth()
        {
            return ObjectsBlowUp.OrderBy(x => x.GetHealth());
        }

        public IEnumerable<IReceiveDamageable> GetObjectsBlowUpArea(Vector3 center, float radius)
        {
            return ObjectsBlowUp.Where(x => Vector3.Distance(center, x.GetTransform.position) <= radius);
        }

        private void Start()
        {
            if (!photonView.IsMine) return;

            AsyncMessageBroker.Default.Subscribe<BlockUpArgs>(args =>
            {
                return Observable.Return(args.ObjectHasBeenBlowUp)
                    .ForEachAsync(obj => { ObjectsBlowUp.Add(obj); });
            });

            AsyncMessageBroker.Default.Subscribe<BlockDownArgs>(args =>
            {
                return Observable.Return(args.ObjectHasBeenBlowDown)
                    .ForEachAsync(obj => { ObjectsBlowUp.Remove(obj); });
            });
        }
    }
}