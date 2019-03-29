using UnityEngine;

namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicHandle : AttackBasicHandler
    {
        protected override void ApplyDamage(IInputMessage message)
        {
            if(message.ObjectReceive == null) return;
            var forward = (message.ObjectReceive.GetTransform.position - transform.position).normalized;
            ChampionTransform.Forward = forward;
            var newMessage = new InputMessage(message.ObjectReceive, GetPhysicDamageCurrent(), GetMagicDamageCurrent(), message.Direction);
            message.ObjectReceive.TakeDamage(this.CreateDamageMessage(newMessage));
        }
    }
}