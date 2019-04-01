using System.Linq;
using ExtraLinq;
using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class SphereAttackBehaviour : SkillBehaviour
    {
        [SerializeField] private LayerMask layerMaskTarget = Physics.AllLayers;

        private LayerMask LayerMaskTarget => layerMaskTarget;

        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receivers = gameObject.GetSphereReceiveDamageable(SkillConfig.Range.Value, LayerMaskTarget);
            ActiveSkillSubject.OnNext(receivers);
            Debug.Log(receivers.IsNullOrEmpty());
            if (receivers.IsNullOrEmpty()) return;
            var damageMessage = new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value);
            receivers.ToList().ForEach(x => x.TakeDamage(damageMessage));
        }
    }
}