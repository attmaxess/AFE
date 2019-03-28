using ControlFreak2;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class BtnSkill_CounttimeState : BtnSkillBase
{
    float countmax = 1;
    public override void StartState()
    {
        base.StartState();

        btnSkillUI.EnableBtn(false);

        countmax = btnSkillUI.countTime;
        btnSkillUI.number.gameObject.SetActive(true);
    }

    public override BtnState UpdateState()
    {
        btnSkillUI.countTime = btnSkillUI.countTime - 0.1f;
        btnSkillUI.touchJoystickSprite.GetComponent<Image>().fillAmount = 1 - btnSkillUI.countTime / countmax;
        btnSkillUI.number.text = Math.Round(btnSkillUI.countTime, 2).ToString();
        if (btnSkillUI.countTime <= 0)
        {
            btnSkillUI.canUseSkill = true;
        }

        if (btnSkillUI.canUseSkill)
            nextState = BtnState.Enable;

        return base.UpdateState();
    }

    public override void StopState()
    {
        btnSkillUI.canUseSkill = true;
        btnSkillUI.isCountTime = false;

        btnSkillUI.EnableBtn(true);

        btnSkillUI.touchJoystickSprite.GetComponent<Image>().fillAmount = 1;
        base.StopState();
        btnSkillUI.number.gameObject.SetActive(false);
    }
}
