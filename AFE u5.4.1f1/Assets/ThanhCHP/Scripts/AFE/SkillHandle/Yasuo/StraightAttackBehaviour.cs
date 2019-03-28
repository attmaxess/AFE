using System;
using ExtraLinq;
using Photon.Pun;
using UniRx;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class StraightAttackBehaviour : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            Observable.Timer(TimeSpan.FromMilliseconds(100)).Subscribe(_ =>
            {
                var receiver =
                    gameObject.GetAllReceiverDamageNearestByRayCastAll(inputMessage.Direction, SkillConfig.Range.Value,
                        LayerMaskTarget);
                ActiveSkillSubject.OnNext(receiver);
                if (receiver.IsNullOrEmpty()) return;
                var damageMessage = new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value);
                receiver.ForEach(x => x.TakeDamage(damageMessage));
            });
        }
    }
}