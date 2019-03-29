using Photon.Pun;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    [DisallowMultipleComponent]
    public class SyncTransformImmediately : MonoBehaviourPun
    {
        private IChampionTransform ChampionTransform { get; set; }

        private void Awake()
        {
            ChampionTransform = GetComponent<IChampionTransform>();
        }

        public void SyncPosition(Vector3 position)
        {
            photonView.RPC("SyncPositionRPC", RpcTarget.All, position);
        }

        [PunRPC]
        private void SyncPositionRPC(Vector3 position)
        {
            transform.position = position;
        }
        
        public void SyncRotationWithDirection(Vector3 direction)
        {
            photonView.RPC("SyncRotationWithDirectionRPC", RpcTarget.All, direction);
        }

        [PunRPC]
        private void SyncRotationWithDirectionRPC(Vector3 direction)
        {
            ChampionTransform.Forward = direction;
        }

        public void SyncLocalScale(Vector3 localScale)
        {
            photonView.RPC("SyncLocalScaleRPC", RpcTarget.All, localScale);
        }

        [PunRPC]
        private void SyncLocalScaleRPC(Vector3 localScale)
        {
            transform.localScale = localScale;
        }
    }
}