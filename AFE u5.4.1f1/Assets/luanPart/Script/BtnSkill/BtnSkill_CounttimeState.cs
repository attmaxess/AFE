public class BtnSkill_CounttimeState : BtnSkillBase
{
    public override BtnState Update()
    {
        if (btnSkillUI.canUseSkill)
            nextState = BtnState.Disable;

        if (!btnSkillUI.canUseSkill)
            nextState = BtnState.Enable;

        return base.Update();
    }
}
