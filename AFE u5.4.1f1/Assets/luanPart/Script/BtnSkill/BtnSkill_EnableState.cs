public class BtnSkill_EnableState : BtnSkillBase
{
    public override BtnState UpdateState()
    {
        if (!btnSkillUI.canUseSkill)
            nextState = BtnState.Disable;

        if (btnSkillUI.isCountTime)
            nextState = BtnState.CountTime;

        return base.UpdateState();
    }
}
