using System;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1A : SkillBehaviour
    {
        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receive = gameObject.ReceiverDamageNearestByRayCast(inputMessage.Direction, SkillConfig.RangePerLevel);
            receive.TakeDamage(new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
        }
    }
}