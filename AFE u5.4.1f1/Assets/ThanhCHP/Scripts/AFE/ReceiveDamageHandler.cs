using System;
using Photon.Pun;
using UniRx;

namespace Com.Beetsoft.AFE
{
    public class ReceiveDamageHandler : MonoBehaviourPun,
        IPunObservable,
        IReceiveDamageable,
        IReceiveDamageObserver,
        IInitialize<IChampionConfig>
    {
        private Subject<IDamageMessage> DamageMessageSubject { get; } = new Subject<IDamageMessage>();

        private IChampionConfig ChampionConfig { get; set; }

        public void TakeDamage(IDamageMessage message)
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
            if(!photonView.IsMine) return;
            
            this.OnTakeDamageAsObservable()
                .Subscribe(HandleWhenReceiveDamage);
        }

        private void HandleWhenReceiveDamage(IDamageMessage damageMessage)
        {
            var physicDamageReceive = this.GetDamageReceive(damageMessage.PhysicDamage, ChampionConfig.Armor.Value);
            var magicDamageReceive = this.GetDamageReceive(damageMessage.MagicDamage, ChampionConfig.MagicResist.Value);
            ChampionConfig.Health.Value -= physicDamageReceive;
            ChampionConfig.Health.Value -= magicDamageReceive;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(ChampionConfig.Health.Value);
            }
            else if (stream.IsReading)
            {
                var health = (float)stream.ReceiveNext();
                ChampionConfig.Health.Value = health;
            }
        }
    }
}