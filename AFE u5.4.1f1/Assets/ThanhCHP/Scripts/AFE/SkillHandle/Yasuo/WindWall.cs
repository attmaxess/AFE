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
                    if (other.GetComponent<IDamageBullet>() != null)
                    {
                        other.GetComponent<IReceiveDamageable>().GetDamageReceive(10, 0);
                    }
                });
        }
        internal override void OnSpawn(Vector3 startPos, Vector3 target, IDamageMessage damageMessage)
        {
            movable.MoveToDir(startPos, target);
        }
    }
}