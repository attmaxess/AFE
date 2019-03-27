using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class InstantiateObjectSkillBehavior : SkillBehaviour
    {
        [SerializeField] private ObjectElementSkillBehaviour objectPrefab;

        private ObjectElementSkillBehaviour ObjectPrefab => objectPrefab;

        private ObjectPoolSkillBehaviour ObjectPool { get; set; }

        private void Awake()
        {
            ObjectPool = new ObjectPoolSkillBehaviour(photonView, ObjectPrefab, transform);
        }

        private void Start()
        {
            ObjectPool.PreloadAsync(1, 1).Subscribe();
        }

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            if (inputMessage.ObjectReceive != null)
                photonView.RPC("ActiveSkillRPC", RpcTarget.All, inputMessage.Direction,
                    inputMessage.ObjectReceive.ViewID);
            else
                photonView.RPC("ActiveSkillRPC", RpcTarget.All, inputMessage.Direction);
        }

        [PunRPC]
        protected virtual void ActiveSkillRPC(Vector3 direction, int viewIdTarget)
        {
            ObjectPool.RentAsync().Subscribe();
        }

        [PunRPC]
        protected virtual void ActiveSkillRPC(Vector3 direction)
        {
            ObjectPool.RentAsync().Subscribe();
        }
    }
}