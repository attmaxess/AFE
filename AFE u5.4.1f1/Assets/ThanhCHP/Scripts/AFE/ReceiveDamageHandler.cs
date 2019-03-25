using System;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class ReceiveDamageHandler : MonoBehaviourPun,
        IReceiveDamageable,
        IReceiveDamageObserver,
        IInitialize<IChampionConfig>
    {
        private Subject<IDamageMessage> DamageMessageSubject { get; } = new Subject<IDamageMessage>();

        private IChampionConfig ChampionConfig { get; set; }

        Transform IReceiveDamageable.GetTransform => transform;

        void IReceiveDamageable.TakeDamage(IDamageMessage message)
        {
            DamageMessageSubject.OnNext(message);
        }

        public IObservable<IDamageMessage> OnTakeDamageAsObservable()
        {
            return DamageMessageSubject;
        }

        public void Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }

        private void Start()
        {
            this.OnTakeDamageAsObservable()
                .Subscribe(HandleWhenReceiveDamage);
            
            if (!photonView.IsMine) return;
            
            this.ChampionConfig.Health
                .Subscribe(x => Debug.Log(x));
        }

        private void HandleWhenReceiveDamage(IDamageMessage damageMessage)
        {
            var physicDamageReceive = this.GetDamageReceive(damageMessage.PhysicDamage, ChampionConfig.Armor.Value);
            var magicDamageReceive = this.GetDamageReceive(damageMessage.MagicDamage, ChampionConfig.MagicResist.Value);
            
            photonView.RPC("HandleHealth", RpcTarget.All, physicDamageReceive, magicDamageReceive);
        }

        [PunRPC]
        private void HandleHealth(float physicDamageReceive, float magicDamageReceive)
        {
            ChampionConfig.Health.Value -= physicDamageReceive;
            ChampionConfig.Health.Value -= magicDamageReceive;
        }
    }
}