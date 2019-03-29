using System;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    [DisallowMultipleComponent]
    public class SyncTweenRPC : MonoBehaviourPun
    {
        public enum SyncMode
        {
            Position,
            Rotation,
            LocalScale
        }

        public event Action OnSyncPositionComplete;
        public event Action OnSyncRotationComplete;
        public event Action OnSyncLocalScaleComplete;

        private void Start()
        {
            OnSyncPositionComplete += () => GetComponent<PhotonTransformView>().enabled = true;
            OnSyncRotationComplete += () => GetComponent<PhotonTransformView>().enabled = true;
            OnSyncLocalScaleComplete += () => GetComponent<PhotonTransformView>().enabled = true;
        }

        public void SyncVectorTween(SyncMode syncMode, Vector3 start, Vector3 end, float duration,
            ObservableTween.EaseType easeType,
            ObservableTween.LoopType loopType = ObservableTween.LoopType.None)
        {
            GetComponent<PhotonTransformView>().enabled = false;
            switch (syncMode)
            {
                case SyncMode.Position:
                    photonView.RPC("SyncPositionTweenRpc", RpcTarget.All, start, end, duration, easeType, loopType);
                    break;
                case SyncMode.Rotation:
                    photonView.RPC("SyncRotationTweenRpc", RpcTarget.All, start, end, duration, easeType, loopType);
                    break;
                case SyncMode.LocalScale:
                    photonView.RPC("SyncLocalScaleTweenRpc", RpcTarget.All, start, end, duration, easeType, loopType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(syncMode), syncMode, null);
            }
        }

        [PunRPC]
        private void SyncPositionTweenRpc(Vector3 start, Vector3 end, float duration, ObservableTween.EaseType easeType,
            ObservableTween.LoopType loopType = ObservableTween.LoopType.None)
        {
            ObservableTween.Tween(start, end, duration, easeType, loopType, OnSyncPositionComplete)
                .Subscribe(rate => transform.position = rate);
        }

        [PunRPC]
        private void SyncRotationTweenRpc(Vector3 start, Vector3 end, float duration, ObservableTween.EaseType easeType,
            ObservableTween.LoopType loopType = ObservableTween.LoopType.None)
        {
            ObservableTween.Tween(start, end, duration, easeType, loopType, OnSyncRotationComplete)
                .Subscribe(rate => transform.forward = rate);
        }

        [PunRPC]
        private void SyncLocalScaleTweenRpc(Vector3 start, Vector3 end, float duration,
            ObservableTween.EaseType easeType,
            ObservableTween.LoopType loopType = ObservableTween.LoopType.None)
        {
            ObservableTween.Tween(start, end, duration, easeType, loopType, OnSyncLocalScaleComplete)
                .Subscribe(rate => transform.localScale = rate);
        }
    }
}