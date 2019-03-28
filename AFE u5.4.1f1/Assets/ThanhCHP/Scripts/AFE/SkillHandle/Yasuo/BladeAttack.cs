using UnityEngine;
using AnimeRx;
using UniRx;
using System.Linq;

namespace Com.Beetsoft.AFE
{
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

            ObservableTween.Tween(transform.position, nearestReceiver.GetTransform.position, 1f, ObservableTween.EaseType.Linear)
         .Subscribe(rate =>
         {
             transform.position = rate;
         });
        }
    }
}