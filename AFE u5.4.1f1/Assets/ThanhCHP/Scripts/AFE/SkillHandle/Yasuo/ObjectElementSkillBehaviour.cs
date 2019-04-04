using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class ObjectElementSkillBehaviour : MonoBehaviour, ITriggerObject
    {
        protected IMovable movable;

        public PhotonView PhotonView { protected get; set; }
        public ObjectPoolSkillBehaviour ObjectPool { protected get; set; }

        protected int IdIgnore { get; private set; }

        public void SetIdIgnore(int id)
        {
            IdIgnore = id;
        }

        protected virtual void Awake()
        {
            movable = GetComponent<IMovable>();
        }

        public void ReturnPool()
        {
            ObjectPool.Return(this);
        }

        internal virtual void OnSpawn(Vector3 startPos, Vector3 direction, IDamageMessage damageMessage)
        {
            
        }
    }
    
    
}