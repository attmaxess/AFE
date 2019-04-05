using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class SpawnObjectWindWall : SkillBehaviour
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
            ActiveSkillSubject.OnNext(new[] { inputMessage.ObjectReceive });
            if (inputMessage.ObjectReceive != null)
                photonView.RPC("SpawnWindWall", RpcTarget.All, inputMessage.Direction,
                    inputMessage.ObjectReceive.ViewID);
            else
                photonView.RPC("SpawnWindWall", RpcTarget.All, inputMessage.Direction);
        }
        [PunRPC]
        public virtual void SpawnWindWall(Vector3 direction)
        {
            Debug.Log("SpawnWindWall");
            ObjectPool.RentAsync().Subscribe(windWall =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                windWall.OnSpawn(transform.position, target, CreateDamageMessage());
                windWall.SetIdIgnore(transform.GetInstanceID());
            });
        }
        [PunRPC]
        public virtual void SpawnWindWall(Vector3 direction, int viewIdTarget)
        {
            Debug.Log("SpawnWindWall");
            ObjectPool.RentAsync().Subscribe(windWall =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                windWall.OnSpawn(transform.position, target, CreateDamageMessage());
                windWall.SetIdIgnore(transform.GetInstanceID());
            });
        }
    }

}