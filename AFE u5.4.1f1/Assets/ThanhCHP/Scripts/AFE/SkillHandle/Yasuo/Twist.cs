using System;
using Photon.Pun;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class Twist : ObjectElementSkillBehaviour
    {
        [SerializeField] private float knockUpTime = 0.7f;
        [SerializeField] private ObjectElementSkillBehaviour effectPoolPrefabs;

        private float KnockUpTime => knockUpTime;

        private IDamageMessage DamageMessageCurrent { get; set; }

        private ObjectPoolSkillBehaviour EffectPool { get; set; }

        private ObjectElementSkillBehaviour EffectPoolPrefabs => effectPoolPrefabs;

        protected override void Awake()
        {
            base.Awake();
            EffectPool = new ObjectPoolSkillBehaviour(PhotonView, EffectPoolPrefabs, transform);
        }

        private void Start()
        {
            if (!PhotonView.IsMine) return;
            this.OnTriggerEnterAsObservable()
                .Where(other => other.transform.GetInstanceID() != IdIgnore)
                .Subscribe(other =>
                {
                    Debug.Log("Twist");
                    var k = other.GetComponent<IKnockUpable>();
                    k?.BlowUp(KnockUpTime);
                    var receiver = other.GetComponent<IReceiveDamageable>();
                    receiver?.TakeDamage(DamageMessageCurrent);
                    PhotonView.RPC("SpawnTwistChildRpc", RpcTarget.All, other.transform.position);
                });
        }
        //
        //        internal override void OnSpawn(Vector3 startPos, Vector3 direction)
        //        {
        //        }

        internal override void OnSpawn(Vector3 startPos, Vector3 target, IDamageMessage damageMessage)
        {
            movable.MoveToDir(startPos, target);
            DamageMessageCurrent = damageMessage;
        }

        [PunRPC]
        private void SpawnTwistChildRpc(Vector3 position)
        {
            EffectPool.RentAsync().Subscribe(x => { x.transform.position = position; });
        }
    }
}