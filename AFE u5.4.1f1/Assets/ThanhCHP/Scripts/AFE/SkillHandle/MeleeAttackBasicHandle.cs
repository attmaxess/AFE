namespace Com.Beetsoft.AFE
{
    public class MeleeAttackBasicHandle : AttackBasicHandler
    {
        protected override void ApplyDamage(ISkillMessage message)
        {
            message.ObjectReceive.TakeDamage(this.CreateDamageMessage(message));
        }
    }
}