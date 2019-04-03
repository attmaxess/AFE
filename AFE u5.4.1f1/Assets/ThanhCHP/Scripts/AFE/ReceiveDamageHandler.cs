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

        public void Initialize(IChampionConfig init)
        {
            ChampionConfig = init;
        }

        Transform IReceiveDamageable.GetTransform => transform;
        int IReceiveDamageable.ViewID => photonView.ViewID;

        float IReceiveDamageable.GetHealth()
        {
            return ChampionConfig?.Health.Value ?? 999999;
        }

        void IReceiveDamageable.TakeDamage(IDamageMessage message)
        {
            DamageMessageSubject.OnNext(message);
        }

        public IObservable<IDamageMessage> OnTakeDamageAsObservable()
        {
            return DamageMessageSubject;
        }

        private void Start()
        {
            OnTakeDamageAsObservable()
                .Subscribe(HandleWhenReceiveDamage);

            if (!photonView.IsMine) return;

            ChampionConfig.Health
                .Subscribe(x => Debug.Log(x));
        }

        private void HandleWhenReceiveDamage(IDamageMessage damageMessage)
        {
            if (ChampionConfig == null)
            {
                Debug.LogError("Set ChampionConfig");
                return;
            }

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