﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ControlFreak2;
using UniRx;
using UniRx.Triggers;
using TMPro;
using UnityEngine.UI;
using Com.Beetsoft.AFE;
using System.Linq;

public struct MessagePlayerData
{
    public int hp;
    public int isAttack;

    public MessagePlayerData(int hp, int isAttack)
    {
        this.hp = hp;
        this.isAttack = isAttack;
    }
}

public class BtnSkillUI : MonoBehaviour
{
    public Dictionary<BtnState, BtnSkillBase> btnSkillBase = new Dictionary<BtnState, BtnSkillBase>();
    BtnSkillBase curState;
    public BtnState btnState;
    public bool canUseSkill;
    public bool isCountTime;
    public TextMeshProUGUI number;
    public TouchJoystickSpriteAnimator touchJoystickSprite;
    public enum SkillType
    {
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4
    }

    public SkillType skillType;
    public SkillHandler skillHandler;

    public float countTime;
    public Transform mainCharacter;

    private void Start()
    {
        canUseSkill = true;
        isCountTime = false;

        var btns = GetComponents<BtnSkillBase>();
        foreach (var item in btns)
        {
            item.btnSkillUI = this;
            btnSkillBase.Add(item.state, item);
        }
        foreach (var item in btnSkillBase)
        {
            if (item.Key == BtnState.Enable)
            {
                btnState = BtnState.Enable;
                curState = item.Value;
                curState.StartState();
            }
        }


        MessageBroker.Default.Receive<MassageSpawnNewCharacter>().Subscribe(MessageBroker =>
        {
            MessageBroker.mainCharacter.Subscribe(character =>
            {
                mainCharacter = character;
                if (mainCharacter != null)
                {
                    FinDSkillReader();
                }
            });
        });
    }
    public void FinDSkillReader()
    {

        TestYasuo yasuoMine = mainCharacter.GetComponent<TestYasuo>();
        if (yasuoMine == null) return;
        if (skillType == SkillType.Skill_1)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                var implement = item as ISkillSpell_1;
                if (implement != null)
                {
                    skillHandler = item;
                }
            }
        }
        if (skillType == SkillType.Skill_2)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                var implement = item as ISkillSpell_2;
                if (implement != null)
                {
                    skillHandler = item;
                }
            }
        }
        if (skillType == SkillType.Skill_3)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                var implement = item as ISkillSpell_3;
                if (implement != null)
                {
                    skillHandler = item;
                }
            }
        }
        if (skillType == SkillType.Skill_4)
        {
            var _l = yasuoMine.GetComponents<SkillHandler>();
            foreach (var item in _l)
            {
                var implement = item as ISkillSpell_4;
                if (implement != null)
                {
                    skillHandler = item;
                }
            }
        }

        skillHandler.OnReceiveSkillMessageOutputAsObservable()
           .Subscribe(_skillHandle =>
           {
               if (_skillHandle == null)
               {
                   Debug.Log("_skillHandle == null");
                   return;
               }
               else
               {
                   isCountTime = true;
                   canUseSkill = false;
                   countTime = _skillHandle.Cooldown;
                   if (_skillHandle.Icon != null)
                       touchJoystickSprite.SetSprite(_skillHandle.Icon);
               }
           });
    }

    private void Update()
    {
        if (mainCharacter == null) return;
        SwitchState(curState.UpdateState());

    }

    void SwitchState(BtnState nextState)
    {
        if (nextState != BtnState.None && nextState != curState.state)
        {
            btnState = nextState;
            curState.StopState();
            curState = btnSkillBase[nextState];
            curState.StartState();
        }
    }

    public void EnableBtn(bool enable)
    {

        touchJoystickSprite.GetComponentInParent<TouchJoystick>().touchPressureBinding.enabled = enable;
        touchJoystickSprite.GetComponentInParent<TouchJoystick>().joyStateBinding.enabled = enable;
    }

}