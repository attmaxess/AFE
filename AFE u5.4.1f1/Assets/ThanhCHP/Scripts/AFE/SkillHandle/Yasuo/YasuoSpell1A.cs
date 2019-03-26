using System;
using System.Linq;

namespace Com.Beetsoft.AFE
{
    public class YasuoSpell1A : SkillBehaviour
    {
        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receive = gameObject.ReceiverDamageNearestByRayCast(inputMessage.Direction, SkillConfig.Range.Value);
            receive?.TakeDamage(new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value));
            ActiveSkillSubject.OnNext(new[] { receive });
        }
    }
}