using System;
using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public abstract class ObjectElementSkillBehaviour : MonoBehaviour
    {
        protected TriggerObject trigger;
        protected IMovable movable;

        private void Awake()
        {
            trigger = GetComponent<TriggerObject>();
            movable = GetComponent<IMovable>();
        }
        public PhotonView PhotonView { get; set; }
        public ObjectPoolSkillBehaviour ObjectPool { get; set; }

        public void ReturnPool()
        {
            ObjectPool.Return(this);
        }

        internal abstract void OnSpawn(Vector3 startPos, Vector3 direction);

        internal abstract void SetIdIgnore(int value);
    }
}