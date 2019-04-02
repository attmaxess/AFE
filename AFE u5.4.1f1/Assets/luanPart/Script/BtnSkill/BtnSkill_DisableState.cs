public class BtnSkill_DisableState : BtnSkillBase
{
    public override void StartState()
    {
        base.StartState();
        btnSkillUI.EnableBtn(false);
    }

    public override BtnState UpdateState()
    {
        if (btnSkillUI.canUseSkill)
            nextState = BtnState.Enable;

        if (btnSkillUI.isCountTime)
            nextState = BtnState.CountTime;

        return base.UpdateState();
    }

    public override void StopState()
    {
        base.StopState();
        btnSkillUI.EnableBtn(true);
    }
}
