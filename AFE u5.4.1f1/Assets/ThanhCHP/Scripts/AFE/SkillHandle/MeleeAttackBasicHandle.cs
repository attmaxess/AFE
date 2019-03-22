using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicHandle : AttackBasicHandler
    {
        protected override void ApplyDamage(ISkillMessage message)
        {
            Debug.Log(message.ObjectReceive);
            if(message.ObjectReceive == null) return;
            var newMessage = new SkillMessage(message.ObjectReceive, GetPhysicDamageBonus(), GetMagicDamageBonus(), message.Direction);
            message.ObjectReceive.TakeDamage(this.CreateDamageMessage(newMessage));
        }
    }
}