namespace Com.Beetsoft.AFE
{
    public class AttackObjectSelectedSkillBehaviour : SkillBehaviour
    {
        public override void ActiveSkill(IInputMessage inputMessage)
        {
            ActiveSkillSubject.OnNext(inputMessage.ObjectReceive != null ? new[] {inputMessage.ObjectReceive} : null);
        }
    }
}