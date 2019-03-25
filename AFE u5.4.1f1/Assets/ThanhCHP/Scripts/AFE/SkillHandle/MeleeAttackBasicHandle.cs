using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicHandle : AttackBasicHandler
    {
        protected override void ApplyDamage(ISkillMessage message)
        {
            if(message.ObjectReceive == null) return;
            var newMessage = new SkillMessage(message.ObjectReceive, GetPhysicDamageCurrent(), GetMagicDamageCurrent(), message.Direction);
            message.ObjectReceive.TakeDamage(this.CreateDamageMessage(newMessage));
        }
    }
}