using System;
using Photon.Pun;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Com.Beetsoft.AFE
{
    public class ObjectPoolSkillBehaviour : AsyncObjectPool<ObjectElementSkillBehaviour>
    {
        private PhotonView PhotonView { get; }

        private ObjectElementSkillBehaviour ObjectPrefab { get; }

        private Transform TransformParent { get; }

        protected override IObservable<ObjectElementSkillBehaviour> CreateInstanceAsync()
        {
            var obj = Object.Instantiate(ObjectPrefab, TransformParent);
            obj.PhotonView = PhotonView;
            obj.ObjectPool = this;
            return Observable.Return(obj);
        }

        public ObjectPoolSkillBehaviour(PhotonView photonView, ObjectElementSkillBehaviour objectPrefab,
            Transform transformParent)
        {
            PhotonView = photonView;
            ObjectPrefab = objectPrefab;
            TransformParent = transformParent;
        }

        protected override void OnBeforeRent(ObjectElementSkillBehaviour instance)
        {
            instance.transform.parent = null;
            base.OnBeforeRent(instance);
        }

        protected override void OnBeforeReturn(ObjectElementSkillBehaviour instance)
        {
            base.OnBeforeReturn(instance);
            var transform = instance.transform;
            transform.parent = TransformParent;
            transform.localPosition = Vector3.zero;
        }
    }
}