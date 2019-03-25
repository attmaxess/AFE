using UnityEngine;
using UnityEngine.UI;

public class BtnSkill_CounttimeState : BtnSkillBase
{
    float countmax = 1;
    public override void StartState()
    {
        base.StartState();
        countmax = btnSkillUI.countTime;
        btnSkillUI.number.gameObject.SetActive(true);
    }

    public override BtnState UpdateState()
    {
        btnSkillUI.countTime = btnSkillUI.countTime - Time.deltaTime;
        btnSkillUI.touchJoystickSprite.GetComponent<Image>().fillAmount = 1 - btnSkillUI.countTime / countmax;
        btnSkillUI.number.text = btnSkillUI.countTime.ToString();
        if (btnSkillUI.countTime <= 0)
        {
            btnSkillUI.canUseSkill = true;
        }

        if (btnSkillUI.canUseSkill)
            nextState = BtnState.Enable;

        if (!btnSkillUI.canUseSkill)
            nextState = BtnState.Disable;

        return base.UpdateState();
    }

    public override void StopState()
    {
        btnSkillUI.touchJoystickSprite.GetComponent<Image>().fillAmount = 1;
        base.StopState();
        btnSkillUI.number.gameObject.SetActive(false);
    }
}
