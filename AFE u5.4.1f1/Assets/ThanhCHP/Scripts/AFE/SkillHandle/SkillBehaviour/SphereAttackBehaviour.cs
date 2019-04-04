using System.Linq;
using ExtraLinq;
using Photon.Pun;
using UnityEngine;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class SphereAttackBehaviour : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget = Physics.AllLayers;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            photonView.RPC("SpawnEffectSphereAttackRpc", RpcTarget.All, transform.position);
            var receivers = gameObject.GetSphereReceiveDamageable(SkillConfig.Range.Value, LayerMaskTarget);
            ActiveSkillSubject.OnNext(receivers);
            Debug.Log(receivers.IsNullOrEmpty());
            if (receivers.IsNullOrEmpty()) return;
            receivers.ToList().ForEach(x => x.TakeDamage(CreateDamageMessage()));
        }

        [PunRPC]
        private void SpawnEffectSphereAttackRpc(Vector3 position)
        {
            EffectPool.RentAsync().Subscribe(x => x.transform.position = position);
        }
    }
}