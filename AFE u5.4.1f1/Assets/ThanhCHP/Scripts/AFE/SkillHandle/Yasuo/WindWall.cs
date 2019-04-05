using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class WindWall : ObjectElementSkillBehaviour
    {

        //        internal override void OnSpawn(Vector3 startPos, Vector3 direction)
        //        {
        //            movable.MoveToDir(startPos, direction);
        //        }

        private void Start()
        {
            if (PhotonView.IsMine) return;
            this.OnTriggerEnterAsObservable()
                .Where(other => other.transform.GetInstanceID() != IdIgnore)
                .Subscribe(other =>
                {
                    Debug.Log("WindWall");

                    var damBullet = other.GetComponent<IDamageBullet>();
                    damBullet?.ReturnPool();
                });
        }
        internal override void OnSpawn(Vector3 startPos, Vector3 target, IDamageMessage damageMessage)
        {
            transform.forward = (target - startPos).normalized;
            movable.MoveToDir(startPos, target);
        }
    }
}