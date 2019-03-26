using ExtraLinq;

namespace Com.Beetsoft.AFE
{
    public class StraightAttackBehaviour : SkillBehaviour
    {
        public override void ActiveSkill(IInputMessage inputMessage)
        {
            var receiver =
                gameObject.GetAllReceiverDamageNearestByRayCastAll(inputMessage.Direction, SkillConfig.Range.Value);
            ActiveSkillSubject.OnNext(receiver);
            if (receiver.IsNullOrEmpty()) return;
            var damageMessage = new DamageMessage(SkillConfig.PhysicDamage.Value, SkillConfig.MagicDamage.Value);
            receiver.ForEach(x => x.TakeDamage(damageMessage));
        }
    }
}