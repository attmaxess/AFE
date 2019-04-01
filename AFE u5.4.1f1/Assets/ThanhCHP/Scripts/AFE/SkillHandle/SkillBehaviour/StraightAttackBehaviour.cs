using ExtraLinq;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class StraightAttackBehaviour : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receivers =
                gameObject.GetAllReceiverDamageNearestByRayCastAll(inputMessage.Direction, SkillConfig.Range.Value,
                    LayerMaskTarget);
            ActiveSkillSubject.OnNext(receivers);
            
            if (receivers.IsNullOrEmpty()) return;
            var damageMessage = new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value);
            receivers.ForEach(x => x.TakeDamage(damageMessage));
        }
    }
}