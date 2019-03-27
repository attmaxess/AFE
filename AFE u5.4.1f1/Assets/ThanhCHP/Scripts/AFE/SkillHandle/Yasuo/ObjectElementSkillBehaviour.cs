using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class ObjectElementSkillBehaviour : MonoBehaviour
    {
        public PhotonView PhotonView { get; set; }
        public ObjectPoolSkillBehaviour ObjectPool { get; set; }

        public void ReturnPool()
        {
            ObjectPool.Return(this);
        }
    }
}