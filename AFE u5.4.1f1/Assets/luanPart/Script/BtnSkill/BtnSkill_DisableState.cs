public class BtnSkill_DisableState : BtnSkillBase
{
    public override BtnState UpdateState()
    {
        if (btnSkillUI.canUseSkill)
            nextState = BtnState.Enable;

        if (btnSkillUI.isCountTime)
            nextState = BtnState.CountTime;

        return base.UpdateState();
    }
}
