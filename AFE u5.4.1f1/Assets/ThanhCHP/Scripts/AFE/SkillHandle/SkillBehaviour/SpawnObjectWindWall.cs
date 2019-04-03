using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class SpawnObjectWindWall : InstantiateObjectSkillBehavior
    {
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
            ObjectPool.RentAsync().Subscribe(windWall =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                windWall.OnSpawn(transform.position + direction, target, CreateDamageMessage());
                windWall.SetIdIgnore(transform.GetInstanceID());
            });
        }
        [PunRPC]
        public virtual void SpawnWindWall(Vector3 direction, int viewIdTarget)
        {
            ObjectPool.RentAsync().Subscribe(windWall =>
            {
                Vector3 target = transform.position + direction * SkillConfig.Range.Value;
                windWall.OnSpawn(transform.position + direction, target, CreateDamageMessage());
                windWall.SetIdIgnore(transform.GetInstanceID());
            });
        }
    }

}