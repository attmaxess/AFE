using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ControlFreak2;
using UniRx;

public struct MessagePlayerData
{
    public int hp;
    public int isAttack;
}

public class BtnSkillUI : MonoBehaviour
{
    public Dictionary<BtnState, BtnSkillBase> btnSkillBase = new Dictionary<BtnState, BtnSkillBase>();
    BtnSkillBase curState;

    public bool canUseSkill;
    public bool isCountTime;

    private void Start()
    {
        MessageBroker messageBroker = new MessageBroker();
       // messageBroker.Receive<MessagePlayerData>().Subscribe(_m = >{ return _m; });


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
                curState = item.Value;
                curState.StartState();
            }
        }
    }

    private void Update()
    {
        SwitchState(curState.Update());
    }

    void SwitchState(BtnState nextState)
    {
        if (nextState != BtnState.None && nextState != curState.state)
        {
            curState.StopState();
            curState = btnSkillBase[nextState];
            curState.StartState();
        }
    }
}