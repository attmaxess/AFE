using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class InstantiateObjectSkillBehavior : SkillBehaviour
    {
        [SerializeField] private ObjectElementSkillBehaviour objectPrefab;

        private ObjectElementSkillBehaviour ObjectPrefab => objectPrefab;

        protected ObjectPoolSkillBehaviour ObjectPool { get; set; }

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
            {
                ActiveSkillSubject.OnNext(new[] { inputMessage.ObjectReceive });
                photonView.RPC("ActiveSkillRPC", RpcTarget.All, inputMessage.Direction,
                    inputMessage.ObjectReceive.ViewID);
            }
            else
            {
                ActiveSkillSubject.OnNext(null);
                photonView.RPC("ActiveSkillRPC", RpcTarget.All, inputMessage.Direction);
            }
        }

        [PunRPC]
        protected virtual void ActiveSkillRPC(Vector3 direction, int viewIdTarget)
        {
            Debug.Log("ActiveSkillRPC");
            ObjectPool.RentAsync().Subscribe(twist =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                twist.OnSpawn(transform.position + direction, target, CreateDamageMessage());
                twist.SetIdIgnore(transform.GetInstanceID());
            });
        }

        [PunRPC]
        protected virtual void ActiveSkillRPC(Vector3 direction)
        {
            Debug.Log("ActiveSkillRPC");
            ObjectPool.RentAsync().Subscribe(twist =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                twist.OnSpawn(transform.position + direction, target, CreateDamageMessage());
                twist.SetIdIgnore(transform.GetInstanceID());
            });
        }
    }
}