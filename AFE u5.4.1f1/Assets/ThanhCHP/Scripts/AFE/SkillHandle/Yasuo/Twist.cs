using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class Twist : ObjectElementSkillBehaviour
    {
        private void Start()
        {
            if (PhotonView.IsMine) return;
            this.OnTriggerEnterAsObservable()
                .Where(other => other.transform.GetInstanceID() != IdIgnore)
                .Subscribe(other =>
                {
                    var k = other.GetComponent<IKnockUpable>();
                    k?.BlowUp(0.7f);
                });
        }

        internal override void OnSpawn(Vector3 startPos, Vector3 direction)
        {
            movable.MoveToDir(startPos, direction);
        }
    }
}