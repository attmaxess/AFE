public class BtnSkill_EnableState : BtnSkillBase
{
    public override BtnState Update()
    {
        if (!btnSkillUI.canUseSkill)
            nextState = BtnState.Disable;

        if (btnSkillUI.isCountTime)
            nextState = BtnState.CountTime;

        return base.Update();
    }
}
