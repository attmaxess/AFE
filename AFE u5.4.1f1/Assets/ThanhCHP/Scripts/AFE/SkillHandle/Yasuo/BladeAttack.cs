using UnityEngine;
using AnimeRx;
using UniRx;
using System.Linq;

namespace Com.Beetsoft.AFE
{
    public class IMessageBladeAttack
    {
        public bool isUsing;
        public bool isMine;
        public Transform player;
        public IMessageBladeAttack(bool isUsing, bool isMine, Transform player)
        {
            this.isUsing = isUsing;
            this.isMine = isMine;
            this.player = player;
        }
    }

    public class BladeAttack : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receiver = gameObject.GetAllReceiverDamageNearestByRayCastAll(inputMessage.Direction
                , SkillConfig.Range.Value, LayerMaskTarget);
            ActiveSkillSubject.OnNext(receiver);
            if (receiver == null) return;
            if (photonView.IsMine)
                MessageBroker.Default.Publish<IMessageBladeAttack>(new IMessageBladeAttack(true, photonView.IsMine, transform));

            IReceiveDamageable nearestReceiver = null;
            float dis = -1;
            foreach (var item in receiver)
            {
                if (Vector3.Distance(item.GetTransform.position, transform.position) > dis)
                {
                    dis = Vector3.Distance(item.GetTransform.position, transform.position);
                    nearestReceiver = item;
                }
            }
            Vector3 dir = new Vector3(nearestReceiver.GetTransform.position.x, transform.position.y, nearestReceiver.GetTransform.position.z) - transform.position;
            Vector3 posTarget = new Vector3(dir.x * ChampionConfig.Range.Value, transform.position.y, dir.z * ChampionConfig.Range.Value);
            ObservableTween.Tween(transform.position, posTarget, 3f, ObservableTween.EaseType.Linear)
                .DoOnCompleted(() => { if (photonView.IsMine) MessageBroker.Default.Publish<IMessageBladeAttack>(new IMessageBladeAttack(false, photonView.IsMine, transform)); })
         .Subscribe(rate =>
         {
             transform.position = rate;
         });
        }
    }
}